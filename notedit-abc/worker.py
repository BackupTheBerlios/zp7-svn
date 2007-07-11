import os, sys, threading, os.path, wx, traceback
import convert

class WorkerThread(threading.Thread):
    lock = None
    event = None
    input_text = None
    output_bitmap = None
    quit_flag = False
    
    def __init__(self):
        threading.Thread.__init__(self)
        self.lock = threading.Lock()
        self.event = threading.Event()

    def onerun(self, abc_text):
        abcfile = convert.tmp_file('abc')
        psfile = convert.tmp_file('ps')
        pngfile = convert.tmp_file('png')
        
        open(abcfile, 'wb').write(abc_text)

        convert.run_abc2ps(abcfile, psfile)
        
        convert.run_gsexe(psfile, pngfile, 'pngmono')
        
        if os.path.isfile(pngfile):
            img = wx.Image(pngfile)
            bmp = wx.BitmapFromImage(img)
            print 'Created image, size=%s' % img.GetSize()
            img.Destroy()
            try:
                self.lock.acquire()
                if self.output_bitmap is not None:
                    self.output_bitmap.Destroy() # image is not used by GUI
                self.output_bitmap = bmp
            finally:
                self.lock.release()
        else:
            print 'Error, PNG file not created'
        convert.remove_files(abcfile, psfile, pngfile)
        
    def run(self):
        while not self.quit_flag:
            self.event.wait()
            self.event.clear()
            if self.quit_flag:
                return
            try:
                self.lock.acquire()
                abc_text = self.input_text
                self.input_text = None
            finally:
                self.lock.release()
            
            if abc_text is None:
                continue
            
            try:
                self.onerun(abc_text)
            except:
                traceback.print_exc()
                

    def quit(self):
        self.quit_flag = True
        self.event.set()

    def set_input(self, abc_text):
        try:
            self.lock.acquire()
            self.input_text = abc_text
            self.event.set()
        finally:
            self.lock.release()

    def get_output(self):
        try:
            self.lock.acquire()
            res = self.output_bitmap
            self.output_bitmap = None
            return res
        finally:
            self.lock.release()

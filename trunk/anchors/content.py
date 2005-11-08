import interop

class IContent:
  def on_create_control(self,parent,evtbinder):
    pass
    
  def on_destroy_control(self):
    pass
    
  def on_show(self):
    pass

  def on_hide(self):
    pass

  def get_name(self):
    raise NotImplemented()
    
  def on_destroy_menu(self):
    pass

interop.anchor.define('content',IContent)
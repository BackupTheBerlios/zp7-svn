# -*- coding: UTF-8 -*-

import re

commands=('prev','next','prev5','next5','simple','easy','orig')

chord_regex=re.compile(ur'\[([^\]]*)\]')
tone_names={
    'C':0,
    'C#':1,'Des':1,'Db':1,
    'D':2,
    'Es':3,'D#':3,'Eb':3,
    'E':4,'Fes':4,'Fb':4,
    'F':5,'E#':5,'Fes':5,'Fb':5,
    'F#':6,'Gb':6,'Ges':6,
    'G':7,
    'G#':8,'Ab':8,'As':8,
    'A':9,
    'B':10,'Bb':10,'A#':10,'Hes':10,
    'H':11,'Ces':11,'Cb':11
    }

chord_type_map={
    '':'dur','-':'moll','mi':'moll','m':'moll','m7':'moll7','mi7':'moll7','m6':'moll6','mi6':'moll6',
    '6':'dur6','7':'dur7','9':'dur9','dim':'dim','maj':'maj','7maj':'maj','maj7':'maj','+':'plus','5+':'plus'
}

chord_difs={
    'Cdur':2,'C#dur':5,'Ddur':1,'Esdur':5,'Edur':1,'Fdur':3,'F#dur':5,'Gdur':2,'Asdur':5,'Adur':1,'Bbdur':5,'Hdur':5,
    'Cmoll':4,'C#moll':5,'Dmoll':3,'Esmoll':5,'Emoll':1,'Fmoll':3,'F#moll':5,'Gmoll':4,'Asmoll':5,'Amoll':1,'Bbmoll':5,'Hmoll':5,
    'Cdur7':3,'C#dur7':5,'Ddur7':1,'Esdur7':5,'Edur7':1,'Fdur7':3,'F#dur7':5,'Gdur7':2,'Asdur7':5,'Adur7':1,'Bbdur7':5,'Hdur7':3,
    'Cdur6':2,'C#dur6':5,'Ddur6':2,'Esdur6':5,'Edur6':2,'Fdur6':4,'F#dur6':5,'Gdur6':3,'Asdur6':5,'Adur6':2,'Bbdur6':5,'Hdur6':5,
    'Cmoll7':4,'C#moll7':5,'Dmoll7':2,'Esmoll7':5,'Emoll7':2,'Fmoll7':4,'F#moll7':5,'Gmoll7':4,'Asmoll7':5,'Amoll7':1,'Bbmoll7':5,'Hmoll7':5,
    'Cmoll6':4,'C#moll6':5,'Dmoll6':2,'Esmoll6':2,'Emoll6':4,'Fmoll6':5,'F#moll6':4,'Gmoll6':4,'Asmoll6':5,'Amoll6':1,'Bbmoll6':5,'Hmoll6':5,
    'Cdur9':3,'C#dur9':5,'Ddur9':2,'Esdur9':5,'Edur9':3,'Fdur9':4,'F#dur9':5,'Gdur9':4,'Asdur9':5,'Adur9':1,'Bbdur9':5,'Hdur9':5,
    'Cdim':2,'C#dim':2,'Ddim':2,'Esdim':2,'Edim':2,'Fdim':2,'F#dim':2,'Gdim':2,'Asdim':2,'Adim':2,'Bbdim':2,'Hdim':2,
    'Cmaj':2,'C#maj':5,'Dmaj':2,'Esmaj':5,'Emaj':4,'Fmaj':3,'F#maj':5,'Gmaj':3,'Asmaj':5,'Amaj':1,'Bbmaj':5,'Hmaj':5,
    'Cplus':3,'C#plus':3,'Dplus':3,'Esplus':3,'Eplus':3,'Fplus':3,'F#plus':3,'Gplus':3,'Asplus':3,'Aplus':3,'Bbplus':3,'Hplus':3
}

sorted_ton_keys=tone_names.keys()
sorted_ton_keys.sort(lambda x,y:-cmp(len(x),len(y)))

tone_heights=['C','C#','D','Es','E','F','F#','G','As','A','Bb','H']

def changetr(acttr,text,trtype):
    res=acttr
    if trtype=='next':res+=1
    elif trtype=='prev':res-=1
    elif trtype=='next5':res+=7
    elif trtype=='prev5':res-=7
    elif trtype=='orig':res=0
    elif trtype=='simple':
        simple,easy=getoptimald(text)
        return simple
    elif trtype=='easy':
        simple,easy=getoptimald(text)
        return easy
    while res>=12:res-=12
    while res<0:res+=12
    return res

def splitchord_nolom(chord):
    for ton in sorted_ton_keys:
        if chord.startswith(ton):
            return (tone_names[ton],chord[len(ton):])
    return ()

def splitchord(chord):
    """rtype:(int(hi),str(type)[,int(hisub),str(type2))]"""
    lom=chord.split('/')
    if len(lom)==2: return splitchord_nolom(lom[0])+splitchord_nolom(lom[1])
    if len(lom)==1: return splitchord_nolom(chord)
    return ()

def extractchords(text):
    """rtype:list((hi,normalized_type))"""
    res=[]
    for ch in chord_regex.findall(text):
        ch=splitchord(ch)
        if len(ch)>=2:
            hi=ch[0]
            chtype=ch[1]
            if chord_type_map.has_key(chtype) : chtype=chord_type_map[chtype]
            else : chtype='dur'
            res.append((hi,chtype))
    return res

def getoptimald(text):
    chords=extractchords(text)
    sdif=0x100000
    edif=0x100000
    simplest=0
    easiest=0
    for d in range(12):
        sactdif=0
        eactdif=0
        for hi,chtype in chords:
            chname=tone_heights[(hi+d)%12]+chtype
            dif=chord_difs[chname]
            sactdif+=dif;
            if dif>eactdif : eactdif=dif

        if sactdif<sdif:
            sdif=sactdif
            simplest=d
        if eactdif<edif:
            edif=eactdif
            easiest=d
    return (simplest,easiest)

def transpchord(chord,tr):
    spl=splitchord(chord)
    if len(spl)==2: return tone_heights[(spl[0]+tr)%12]+spl[1]
    if len(spl)==4: return tone_heights[(spl[0]+tr)%12]+spl[1]+'/'+tone_heights[(spl[2]+tr)%12]+spl[3]
    return chord

def transp(text,tr):
    return chord_regex.sub(lambda chord:u'['+transpchord(chord.group(1),tr)+u']',text)

def deletechords(text):
    return chord_regex.sub("",text)

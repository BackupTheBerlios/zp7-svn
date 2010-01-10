import utils.xmlnode

def weekequal(a,b):
    if a.name!=b.name: return False
    if a.attrs!=b.attrs: return False

    if len(a.childs)!=len(b.childs): return False
    for x in a:
        if x.name not in b: return False
        if not weekequal(x,b/x.name): return False

    return True

def xmlmerge(res,add):
    for var in add.attrs: res.attrs[var]=add.attrs[var]
    for child in add:
        idx=res.findsub(child.name)
        if idx>=0: xmlmerge(res.childs[idx],child)
        else: xmlmerge(res.add(child.name),child)

def xmldiff(full,base,diff):
    diff.name=full.name
    for var in full.attrs:
        if full[var]!=base[var]:
            diff[var]=full[var]

    for f in full:
        if not weekequal(f,base/f.name):
            xmldiff(f,base/f.name,diff.add(f.name))


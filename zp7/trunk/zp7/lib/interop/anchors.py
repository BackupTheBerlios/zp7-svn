class Anchor:
    features=[]
    intf=None
    default=None

    def __init__(self,intf):
        self.features=[]
        self.intf=intf

    def add_feature(self,obj):
        assert isinstance(obj,self.intf)
        self.features.append(obj)

    def find(self,name,default=None):
        """returns named feature, default is default feature object or name of default feature (string)"""
        if isinstance(default,basestring): default=self.find(default)
        if not default: default=self.default
        for feature in self.features:
            if feature.name==name: return feature
        return default

    def set_default(self,default):
        self.default=default

    def add_default(self,obj):
        self.add_feature(obj)
        self.set_default(obj)

    def __iter__(self):
        return iter(self.features)

class Anchors:
    _anchors={}

    def __init__(self):
        self._anchors={}

    def define(self,name,intf):
        """defines new anchor

        @type intf: class
        """
        assert not self._anchors.has_key(name)
        self._anchors[name]=Anchor(intf)

    def __getitem__(self,name): 
        return self._anchors[name]


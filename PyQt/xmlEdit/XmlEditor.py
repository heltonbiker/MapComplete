from xml.dom import minidom
from xml.parsers.expat import ExpatError

class XmlEditor(object):
    def __init__(self, xmlstring=""):
        self.string = xmlstring
        try:
            self.tree = minidom.parseString(xmlstring)
        except ExpatError as e:
            raise ValueError(e)

    def to_string(self):
        return self.string
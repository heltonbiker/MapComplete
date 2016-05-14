#!/usr/bin/env python
# coding: utf-8

from xml.dom import minidom

ELEMENT = 1
ATTRIBUTE = 2
TEXT = 3

COMMENT = 8

typenames = {1: "Element",
             2: "Attribute",
             3: "Text",
             4: "CDATA",
             5: "EntityReference",
             6: "Entity",
             7: "ProcessingInstruction",
             8: "Comment",
             9: "Document",
             10: "DocumentType",
             11: "DocumentFragment",
             12: "Notation"}

with open('../kmlsamples.kml') as f:
    xmlstring = f.read()

dom = minidom.parseString(xmlstring)
kml = dom.documentElement

def traverse(node):
    for child in node.childNodes:
        t = child.nodeType
        if t == ELEMENT:
            # print node.nodeName
            # print node.attributes.items()            
            traverse(child)
        elif t == ATTRIBUTE:
            print t
        elif t == TEXT:
            # print [child.nodeValue]
            pass
        elif t == COMMENT:
            print child.nodeValue

traverse(dom)

# def printRecursive(node):
#     nodeType = node.nodeType
#     if nodeType == 1:
#         print "<{0}>".format(node.nodeName)
#     else:
#         print node.nodeName
#     print "type", typenames[node.nodeType]
#     print "value", [node.nodeValue]
#     for childNode in node.childNodes:
#         printRecursive(childNode)
#     print "</{0}>".format(node.nodeName)

# printRecursive(kml)

# print kml.toxml()
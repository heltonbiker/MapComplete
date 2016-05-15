#!/usr/bin/env python
#coding: utf-8

import unittest
from xmlEdit.XmlEditor import *

class XmlEditTest(unittest.TestCase):

    def setUp(self):
        pass

    def tearDown(self):
        pass


    def test_RejectsEmptyString(self):
        with self.assertRaises(ValueError):
            XmlEditor("")

    def test_RejectsUnclosedTag(self):
        invalid_input = "<example><example>"
        with self.assertRaises(ValueError):
            XmlEditor(invalid_input)

    def test_RejectsMismatchedTag(self):
        mismatched_tags = "<example></iximple>"
        with self.assertRaises(ValueError):
            XmlEditor(mismatched_tags)

    def test_ValidStringGoesInAndOutUnmodified(self):
        xmlstring = "<example></example>"
        treein = XmlEditor(xmlstring)
        treeout = treein.to_string()
        self.assertMultiLineEqual(xmlstring, treeout)

    def test_AttributeFormattingIsPreserved(self):
        xmlstring = """
<example one="1"
         two="2"
         three="3"/>""".strip()
        treein = XmlEditor(xmlstring)
        treeout = treein.to_string()
        self.assertMultiLineEqual(xmlstring, treeout)

def main():
    unittest.main()

if __name__ == '__main__':
    main()
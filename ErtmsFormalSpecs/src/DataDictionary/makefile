.phony: Comparer

META=DataDictionary
ADDITIONAL_BUILD_TARGET=Comparer Cleaner

include ../etc/makefile.shared
include ../etc/makefile.xmlb


Comparer: src/Compare/Comparer.cs

src/Compare/Comparer.cs: $(META).xmlb generate_comparer.rcs 
	-$(XMLB_CALL) -SCRIPT generate_comparer.rcs $<


Cleaner: src/Cleaner.cs

src/Cleaner.cs: $(META).xmlb generate_cleaner.rcs 
	-$(XMLB_CALL) -SCRIPT generate_cleaner.rcs $<



.PHONY: all release gitclone nightbuild java clean

all:
	make -C depends all
	make -C src all

release: clean
	make -C src release

gitclone:
	-rm -rf ERTMSFormalSpecs
	../tools/Git/bin/git.exe clone https://github.com/stanpinte/ERTMSFormalSpecs.git
	cp -r ERTMSFormalSpecs/ErtmsFormalSpecs/* .

java:
#	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -config=src/ErtmsSolutions/CSharpToJava.ini
	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -net-templates-dir=cs2j-2013.0.1/NetFramework/ -out-java-dir=java/interpreter/translated -cs-dir=src/ErtmsSolutions
	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -net-templates-dir=cs2j-2013.0.1/NetFramework/ -out-java-dir=java/interpreter/translated -cs-dir=src/XmlBooster/src
	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -net-templates-dir=cs2j-2013.0.1/NetFramework/ -out-java-dir=java/interpreter/translated -cs-dir=src/Utils/src
	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -net-templates-dir=cs2j-2013.0.1/NetFramework/ -out-java-dir=java/interpreter/translated -cs-dir=src/DataDictionary/src
	cs2j-2013.0.1/CS2JTranslator/bin/cs2j.exe -net-templates-dir=cs2j-2013.0.1/NetFramework/ -out-java-dir=java/interpreter/translated -cs-dir=src/DataDictionary/generated

nightbuild: release
	bin/EFSTester.exe doc/specs/subset-026.efs
	echo "done."

clean:
	make -C depends clean
	make -C src clean
	rm -rf distrib distrib.zip
	-mkdir bin

ea_diego:
	git.exe log --author diegofdz --since=7.days --stat --grep 'EA_MODEL\|EA_TEST' --all-match --no-abbrev-commit --date=rfc | grep -v "commit" > ERTMSAcademyReport.txt
	mv ERTMSAcademyReport.txt ./bin

ea_moritz:
	git.exe log --author morido --since=7.days --stat --grep 'EA_MODEL\|EA_TEST' --all-match --no-abbrev-commit --date=rfc | grep -v "commit" > ERTMSAcademyReport.txt
	mv ERTMSAcademyReport.txt ./bin

ea_peyman:
	git.exe log --author Farhangi --since=7.days --stat --grep 'EA_MODEL\|EA_TEST' --all-match --no-abbrev-commit --date=rfc | grep -v "commit" > ERTMSAcademyReport.txt
	mv ERTMSAcademyReport.txt ./bin

ea_james:
	git.exe log --author james --since=7.days --stat --grep 'EA_MODEL\|EA_TEST' --all-match --no-abbrev-commit --date=rfc | grep -v "commit" > ERTMSAcademyReport.txt
	mv ERTMSAcademyReport.txt ./bin

% efsguide.sty
% Generate EFS guidelines
% 2013-Dec-25 Moritz Dorka morido (at) web.de

% --- Class structure: identification part
% ---

\ProvidesClass{efsguide}[2013/12/25 version 0.01 ERTMS Formal Specs Guidelines]
\NeedsTeXFormat{LaTeX2e}

% --- Class structure: declaration of options part
% ---
% This class extends the scrreprt class

%\DeclareOption*{\PassOptionsToClass{\CurrentOption}{scrreprt}}
\DeclareOption{draft}{
  \PassOptionsToClass{\CurrentOption}{scrreprt}
}


% --- Class structure: execution of options part
% ---
\ProcessOptions \relax

% --- Class structure: load base class
% ---
\LoadClass[a4paper, oneside]{scrreprt}

% --- Class structure: define commands
% ---
\newcommand{\docTitle}{Guidelines}
\newcommand{\docSubTitle}{Some Guidelines}


% --- Class structure: setup packages
% ---

\AtBeginDocument{%
  \hypersetup{%
  draft = false,  %link in draft mode as well
  pdftitle = {\docTitle, \docSubTitle},
  pdfkeywords = {\docTitle, Version \vhCurrentVersion from \vhCurrentDate},
  pdfauthor = {\vhAllAuthorsSet},
  colorlinks = true,
  linkcolor = black,
  citecolor = black,
  filecolor = black,
  urlcolor = black
  }
}

\AtBeginDocument{%
  \pagestyle{scrheadings}
  \chead[]{}
  \ifoot[\docTitle, \docSubTitle\ -- Version \vhCurrentVersion]{\docTitle, \docSubTitle\ -- Version \vhCurrentVersion}
  \cfoot[]{}
  \ofoot[\thepage]{\thepage}
}


% --- Class structure: load packages
% ---
\RequirePackage[american]{babel}
\RequirePackage{hyperref}
\RequirePackage{pdfcomment}
\RequirePackage{enumitem}
\RequirePackage[nohyphen]{underscore} %allow for simple _ instead of \_
\RequirePackage[usenames, dvipsnames]{xcolor}
\RequirePackage{xspace}
\RequirePackage[tablegrid]{vhistory}
\RequirePackage[nonumberlist]{glossaries}
\RequirePackage{fixme}
\RequirePackage{ifdraft}
\RequirePackage[lighttt]{lmodern} %add better ttfont including a bold variant
\RequirePackage{listings}
\ifdraft{\RequirePackage[firstpage]{draftwatermark}}{}
\RequirePackage{scrpage2}
\RequirePackage[titletoc]{appendix} %for the word "Appendix" in the TOC



% --- Class structure: specific commands
% ---
\makeatletter
% makes the enumeration item label start with the number
% of the current section/subsection/etc.
\newcommand{\thecurrentlevel}{%
  \ifnum\c@section=0\thechapter\else
  \ifnum\c@subsection=0\thesection\else
  \ifnum\c@subsubsection=0\thesubsection\else
  \thesubsubsection\fi\fi\fi}
\makeatother

\setcounter{secnumdepth}{3}
\setcounter{tocdepth}{3}
\setlength{\parindent}{0em}

\newlist{rules}{enumerate}{3}
\setlist[rules, 1]{leftmargin=*, label=\thecurrentlevel.\arabic*.}
\setlist[rules, 2]{label*=\arabic*.}
\setlist[rules, 3]{label*=\arabic*.}
\let\emph\textsl


\newcommand{\literally}[1]{\mbox{\textsf{\emph{#1}}}}
\newcommand{\indraftmode}[1]{\ifdraft{#1}{}}

\fxsetup{mode=multiuser, theme=color, layout=inline}
\colorlet{fxnote}{Green} 
\colorlet{fxnotefg}{black}
\colorlet{fxnotebg}{LimeGreen} %is not the same as fxnote to keep it readable
\colorlet{fxfatal}{red}
\colorlet{fxfatalbg}{red}
\colorlet{fxfatalfg}{white}
\colorlet{fxwarning}{Plum}
\colorlet{fxwarningbg}{Plum}
\colorlet{fxwarningfg}{White}
% redefine the layout macro:
\makeatletter
\renewcommand*\FXLayoutInline[3]{%
  \@fxdocolon {#3}{%
    \@fxuseface {inline}%
    \colorbox{fx#1bg}{\color {fx#1fg}\ignorespaces \sffamily #2\textsubscript{\emph{#3}}}}   
    }
\makeatother

\setliststyle{AuthorComment} %alter the appearance of the pdfcomments list

\lstdefinelanguage{EFSDSL}
{%
keywords={SUM, AND, APPLY, COUNT, EMPTY, FIRST_IN, FORALL_IN, IN, INITIAL_VALUE, LAST_IN, MAP, NOT, ON, OR, REDUCE, RESULT, THERE_IS_IN, USING, X},
morekeywords=[2]{AddIncrement, Allocate, Available, Before, DecelerationProfile, IntersectAt, Max, Min, MinSurface, Override, RoundToMultiple,  Targets},
morekeywords=[3]{Boolean, Collection, Condition, double, Expression, f, List, ListExpression, ListValue, Statement, StatementList},
morestring=[b]',
sensitive=true}
\lstset{%
language=EFSDSL,
basicstyle=\ttfamily,
keywordstyle=\bfseries,
keywordstyle=[2]\underline,
keywordstyle=[3]\emph,
stringstyle=\sffamily,
showstringspaces=true,
breakatwhitespace=true,
breaklines=true}
\newcommand{\code}[1]{\lstinline$#1$}

\ifdraft{%
\newcommand{\ruleauthor}[2]{\mbox{}\newline\mbox{}\hfill{\footnotesize\textcolor{gray}{\emph{#1, #2}}}\xspace}}
{%
\newcommand{\ruleauthor}[2]{}
}


\newcommand{\addauthor}[2]{%
%setup vhistory
\expandafter\newcommand\csname #1\endcsname{#2}
%setup fixme
\FXRegisterAuthor{#1}{a#1}{#1}
%setup personal PDF Comment
\ifdraft{%
\expandafter\newcommand\csname #1explain\endcsname[1]{\pdfcomment[author=#2, color=yellow, icon=Comment, subject=Comment, open=true, hspace=2em]{##1}}}
{%
\expandafter\newcommand\csname #1explain\endcsname[1]{}
}
}

% --- Class structure: additional draft options
% ---
\AtEndDocument{%
	\indraftmode{
	\newpage
	\phantomsection
	\addcontentsline{toc}{chapter}{List of Corrections}
	\listoffixmes\newpage
	\listofpdfcomments[heading=List of PDF comments]
	}
}

\endinput
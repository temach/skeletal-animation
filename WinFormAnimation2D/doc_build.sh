#!/usr/bin/env bash
pwd && doxygen Doxyfile_paper
(cd latex_paper/ && make )
(cd latex_paper/ && evince refman.pdf & )

latexfolder="/home/artem/Desktop/Uni/coursework/soft_engineer_intro/explain/"
latexfile="/home/artem/Desktop/Uni/coursework/soft_engineer_intro/explain/class_field_method.tex"

# clear latex file
echo "" > "$latexfile"

# copy files from output into documentation folder and add file names to latex file
for fname in latex_paper/class_win_form_animation2_d_1_1_*;
do
  cp "$fname" "${latexfolder}${fname}"
  justname="$(basename -s .tex $fname)"
  echo "\\input{latex_paper/${justname}}" >> "$latexfile"
done

# copy files from output into documentation folder and add file names to latex file
for fname in latex_paper/interface_win_form_animation*;
do
  cp "$fname" "${latexfolder}${fname}"
  justname="$(basename -s .tex $fname)"
  echo "\\input{latex_paper/${justname}}" >> "$latexfile"
done


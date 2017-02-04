#!/bin/bash

sed -e "s/#/$1/g" < ~/scartch/template.txt > $1BlockWrapper.cs

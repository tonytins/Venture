#!/bin/bash

ARCHITECTURE=""
PROJECT="Venture.OS"

case $(uname -m) in
    arm64)   ARCHITECTURE="arm64" ;;
    x86_64) ARCHITECTURE="x64" ;;
esac

cosmos build -a $ARCHITECTURE -p $PROJECT
cosmos run -a $ARCHITECTURE -p $PROJECT
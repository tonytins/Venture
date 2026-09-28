#!/bin/sh

ARCHITECTURE=""
PROJECT="Venture.OS"

# This doesn't yet account for Cosmos
case $(uname -m) in
    arm64)   ARCHITECTURE="arm64" ;;
    x86_64) ARCHITECTURE="x64" ;;
    *) echo "Unsupported architecture."
       exit ;;
esac

cosmos check
cosmos build -a $ARCHITECTURE -p $PROJECT
cosmos run -a $ARCHITECTURE -p $PROJECT
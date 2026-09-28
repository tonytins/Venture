#!/bin/sh

ARCHITECTURE="x64" # Bit of a workaround for macOS on Apple Silicon
PROJECT="Venture.OS"

cosmos check
cosmos build -a $ARCHITECTURE -p $PROJECT
cosmos run -a $ARCHITECTURE -p $PROJECT
#!/bin/bash
cd "Venture.OS" || exit
cosmos build -a arm64
cosmos run -a arm64

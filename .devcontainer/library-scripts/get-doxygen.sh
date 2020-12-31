#!/usr/bin/env bash
#-------------------------------------------------------------------------------------------------------------
# Copyright (c) David Li. All rights reserved.
# Licensed under the MIT License.
#-------------------------------------------------------------------------------------------------------------

# Syntax: ./azcli-debian.sh
set -e

if [ "$(id -u)" -ne 0 ]; then
    echo -e 'Script must be run a root. Use sudo, su, or add "USER root" to your Dockerfile before running this script.'
    exit 1
fi
export DEBIAN_FRONTEND=noninteractive
apt-get update -y
apt-get install doxygen graphviz -y
echo "done installing debian"
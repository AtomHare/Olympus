#!/bin/sh

# =======================================================
#sudo add-apt-repository ppa:bartbes/love-stable -y
sudo apt-get update
sudo apt-get install love luajit libphysfs-dev luarocks libgtk-3-dev -y

# not needed
# luarocks config lua_version 5.1
# luarocks

# required for luarocks install
git config --global url."https://github.com/".insteadOf git://github.com/

luarocks $LUAROCKSPREARGS install --tree=luarocks https://raw.githubusercontent.com/maddie480/lua-subprocess/master/subprocess-scm-1.rockspec $LUAROCKSARGS
luarocks $LUAROCKSPREARGS install --tree=luarocks https://raw.githubusercontent.com/Vexatos/nativefiledialog/master/lua/nfd-scm-1.rockspec $LUAROCKSARGS
luarocks $LUAROCKSPREARGS install --tree=luarocks lsqlite3complete $LUAROCKSARGS

# =======================================================
# build

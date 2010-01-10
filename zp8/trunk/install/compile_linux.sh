cd .dinst
gzip -d zpevnikator-debian.tgz
tar xvf zpevnikator-debian.tar
find . -type d -exec chmod 755 {} \;
find . -type f -exec chmod 644 {} \;
chmod +x ./zpevnikator-debian/usr/bin/zpevnikator
chmod +x ./zpevnikator-debian/usr/bin/daci
chmod +x ./zpevnikator-debian/DEBIAN/postinst
chmod +x ./zpevnikator-debian/DEBIAN/prerm
dpkg -b zpevnikator-debian zpevnikator-debian.deb

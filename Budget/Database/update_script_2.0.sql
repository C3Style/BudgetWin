/*
INSTALLATION SUR UN NOUVEAU MYSQL

Après installation de Easy PHP 16.0, il faut sed rendre dans un DOS dans le répertoire :
C:\Program Files (x86)\EasyPHP-Devserver-16.1\eds-binaries\dbserver\mysql5711x86x171114220008\bin

et il faut exécuter la commande suivante : mysql_upgrade -u root -p --force
Ceci afin de mettre à jour MySQL (Ce n'est pas grave s'il y a des erreurs)

Redémarrer ensuite le serveur MySQL : il doit être au minimum en 5.7.11
*/

ALTER TABLE `operation_t` ADD `OP_ICON` VARCHAR(100) NULL AFTER `OP_LOGIN`; 

ALTER TABLE `recurrence_t` ADD `RE_ID` INT UNSIGNED NOT NULL AUTO_INCREMENT, ADD INDEX (`RE_ID`);
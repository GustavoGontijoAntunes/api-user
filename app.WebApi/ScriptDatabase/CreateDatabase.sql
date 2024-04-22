DROP SCHEMA IF EXISTS `db-app`;
CREATE SCHEMA `db-app`;
USE `db-app`;

DROP TABLE IF EXISTS `profile`;
CREATE TABLE `profile` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `profile_name_unique` (`name`)
) ENGINE=InnoDB;

DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `login` varchar(20) NOT NULL,
  `password` varchar(200) NOT NULL,
  `sessionId` varchar(200) NULL,
  `profileId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_profileUser` (`profileId`),
  CONSTRAINT `FK_profileUser` FOREIGN KEY (`profileId`) REFERENCES `profile` (`id`),
  UNIQUE KEY `user_login_unique` (`login`)
) ENGINE=InnoDB;

DROP TABLE IF EXISTS `permission`;
CREATE TABLE `permission` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `description` varchar(200) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `permission_name_unique` (`name`)
) ENGINE=InnoDB;

DROP TABLE IF EXISTS `profilePermission`;
CREATE TABLE `profilePermission` (
  `id` int NOT NULL AUTO_INCREMENT,
  `profileId` int NOT NULL,
  `permissionId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_profileProfilePermission` (`profileId`),
  CONSTRAINT `FK_profileProfilePermission` FOREIGN KEY (`profileId`) REFERENCES `profile` (`id`) ON DELETE CASCADE,
  KEY `FK_permissionProfilePermission` (`permissionId`),
  CONSTRAINT `FK_permissionProfilePermission` FOREIGN KEY (`permissionId`) REFERENCES `permission` (`id`)
) ENGINE=InnoDB;

INSERT INTO `profile` (`name`) VALUES ('Administrador');

INSERT INTO `permission` (`name`, `description`)
VALUES ('VIS_LOG_AUDIT', 'Permite ao usuário visualizar os dados de log e auditoria do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('POST_PERMISSION', 'Permite ao usuário inserir novas permissões no sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('PUT_PERMISSION', 'Permite ao usuário alterar as permissões do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('DELETE_PERMISSION', 'Permite ao usuário deletar as permissões do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('GET_PERMISSION', 'Permite ao usuário visualizar as permissões do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('POST_PROFILE', 'Permite ao usuário inserir novos perfis no sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('PUT_PROFILE', 'Permite ao usuário alterar os perfis do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('DELETE_PROFILE', 'Permite ao usuário deletar os perfis do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('GET_PROFILE', 'Permite ao usuário visualizar os perfis do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('POST_USER', 'Permite ao usuário inserir novos usuários no sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('PUT_USER', 'Permite ao usuário alterar os usuários do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('DELETE_USER', 'Permite ao usuário deletar os usuários do sistema.');
INSERT INTO `permission` (`name`, `description`)
VALUES ('GET_USER', 'Permite ao usuário visualizar os usuários e permissões do sistema.');

INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 1);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 2);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 3);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 4);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 5);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 6);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 7);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 8);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 9);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 10);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 11);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 12);
INSERT INTO `profilePermission` (`profileId`, `permissionId`) VALUES (1, 13);

INSERT INTO `user` (`id`, `name`, `login`, `password`, `sessionId`, `profileId`)
VALUES (1, 'Administrador', 'Admin', '$2a$10$Lwa9rI2zYaO90IpioOBNMOBqikUkABxrqoxz.KHhhPQGvYm6uvFzW', '', 1); -- Password: admin123
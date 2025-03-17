/* Create the database */
CREATE DATABASE IF NOT EXISTS smart_doorbell;
/* Use the database */
USE smart_doorbell;

/* Create user table with primary key and unique constraint */
CREATE TABLE user(
    Id INT NOT NULL AUTO_INCREMENT,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    registration_code VARCHAR(255) NOT NULL UNIQUE,
    fcm_token VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

/* Create device table with primary key and foreign key */
CREATE TABLE device(
    Id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    registration_code VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT fk_registration_code FOREIGN KEY (registration_code) REFERENCES user (registration_code)
);

/* Create device table with primary key and foreign key */
CREATE TABLE image(
    Id INT NOT NULL AUTO_INCREMENT,
    data LONGBLOB NOT NULL,
    insert_date DATETIME,
    user_id INT NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT fk_user_id FOREIGN KEY (user_id) REFERENCES user (Id)
);

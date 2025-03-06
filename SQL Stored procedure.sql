/* Stored procedures */

/* Add user */
DELIMITER $$

CREATE PROCEDURE AddUser(
IN p_username VARCHAR(255),
IN p_password VARCHAR(255),
IN p_registration_code VARCHAR(255)
)
BEGIN
    INSERT INTO user (username, password, registration_code) VALUE (p_username, p_password, p_registration_code);
END $$

DELIMITER ;

/* Get user by id */
DELIMITER $$

CREATE PROCEDURE GetUserById(
IN p_id INT
)
BEGIN
   SELECT * FROM user WHERE Id = p_id;
END $$

DELIMITER ;

/* Update user */
DELIMITER $$

CREATE PROCEDURE UpdateUser(
IN p_id INT,
IN p_username VARCHAR(255),
IN p_password VARCHAR(255)
)
BEGIN
    Update user
    Set username = p_username, password = p_password
    WHERE Id = p_id;
END $$

DELIMITER ;

/* Add device */
DELIMITER $$

CREATE PROCEDURE AddDevice(
IN p_device_name VARCHAR(255), 
IN p_registration_code VARCHAR(255)
)
BEGIN
    INSERT INTO device (name, registration_code) VALUE (p_device_name, p_registration_code);
END $$

DELIMITER ;

/* Get devices */
DELIMITER $$

CREATE PROCEDURE GetDevices(
    IN p_user_id VARCHAR(255)
)
BEGIN
    -- Declare a variable to store the registration code
    DECLARE p_registration_code VARCHAR(255);
    
    -- Get the registration code for the user
    SELECT registration_code INTO p_registration_code 
    FROM user 
    WHERE id = p_user_id;
    
    -- Check if registration code was found
    IF p_registration_code IS NULL THEN
        -- If no registration code is found, return a message or handle it as needed
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User not found or no registration code associated with user.';
    ELSE
        -- Get devices based on the registration code
        SELECT * FROM device WHERE registration_code = p_registration_code;
    END IF;
END $$

DELIMITER ;



/* Add image */
DELIMITER $$

CREATE PROCEDURE AddImage(
IN p_data LONGBLOB, 
IN p_user_id INT
)
BEGIN
    INSERT INTO image (data, inser_date, user_id) VALUE (p_data, NOW(), p_user_id);
END $$

DELIMITER ;

/* Delete image */
DELIMITER $$

CREATE PROCEDURE DeleteImage(
IN p_id INT 
)
BEGIN
    DELETE FROM image WHERE ID = p_id;
END $$

DELIMITER ;

/* Get image by id */
DELIMITER $$

CREATE PROCEDURE GetImageById(
IN p_id INT
)
BEGIN
   SELECT * FROM image WHERE Id = p_id;
END $$

DELIMITER ;

/* Get all images by id */
DELIMITER $$

CREATE PROCEDURE GetImages(
IN p_user_id INT
)
BEGIN
   SELECT * FROM image WHERE user_id = p_user_id;
END $$

DELIMITER ;


/* Delete images older than 30 days */
DELIMITER $$

CREATE PROCEDURE Delete30DayOld()
BEGIN
    DELETE FROM images
	WHERE date < NOW() - INTERVAL 30 DAY;
END $$

DELIMITER ;

/* Get user by username */
DELIMITER $$

CREATE PROCEDURE GetUserByUsernameuser(
IN p_username varchar(255)
)
BEGIN
 SELECT * FROM user Where username = p_username;
END $$

DELIMITER ;

SET GLOBAL event_scheduler = ON;

CREATE EVENT DeleteOldImagesEvent
ON SCHEDULE EVERY 1 DAY
STARTS CURRENT_DATE + INTERVAL 2 HOUR
DO
CALL Delete30DayOld();

SHOW EVENTS;

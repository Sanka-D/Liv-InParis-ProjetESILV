-- Drop and recreate the database
DROP DATABASE IF EXISTS livinparis;
CREATE DATABASE livinparis;
USE livinparis;

-- Création de la table des utilisateurs
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role ENUM('Admin', 'Manager', 'User') NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insertion d'utilisateurs de test
INSERT INTO users (username, password, role, email) VALUES
('admin', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'Admin', 'admin@livinparis.com'), -- password: admin123
('manager', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'Manager', 'manager@livinparis.com'), -- password: manager123
('user', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'User', 'user@livinparis.com'); -- password: user123

-- Création de la table des droits
CREATE TABLE permissions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    role ENUM('Admin', 'Manager', 'User') NOT NULL,
    can_create BOOLEAN DEFAULT FALSE,
    can_edit BOOLEAN DEFAULT FALSE,
    can_delete BOOLEAN DEFAULT FALSE,
    can_view BOOLEAN DEFAULT TRUE
);

-- Insertion des droits par défaut
INSERT INTO permissions (role, can_create, can_edit, can_delete, can_view) VALUES
('Admin', TRUE, TRUE, TRUE, TRUE),
('Manager', TRUE, TRUE, FALSE, TRUE),
('User', FALSE, FALSE, FALSE, TRUE);

-- Création de la table des cuisiniers
CREATE TABLE cuisiniers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    specialite VARCHAR(100) NOT NULL,
    experience INT NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telephone VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Insertion de cuisiniers de test
INSERT INTO cuisiniers (nom, prenom, specialite, experience, email, telephone) VALUES
('Dupont', 'Jean', 'Cuisine française', 10, 'jean.dupont@cuisine.com', '0612345678'),
('Martin', 'Sophie', 'Cuisine italienne', 8, 'sophie.martin@cuisine.com', '0623456789'),
('Bernard', 'Pierre', 'Cuisine asiatique', 12, 'pierre.bernard@cuisine.com', '0634567890');

-- Création de la table des clients
CREATE TABLE clients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    adresse VARCHAR(200) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telephone VARCHAR(20) NOT NULL,
    preferences VARCHAR(200),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Insertion de clients de test
INSERT INTO clients (nom, prenom, adresse, email, telephone, preferences) VALUES
('Leroy', 'Marie', '15 rue de Paris, 75001', 'marie.leroy@client.com', '0645678901', 'Végétarien'),
('Petit', 'Thomas', '22 avenue des Champs, 75008', 'thomas.petit@client.com', '0656789012', 'Sans gluten'),
('Moreau', 'Julie', '8 boulevard Saint-Germain, 75006', 'julie.moreau@client.com', '0667890123', 'Halal');

-- Création de la table des plats
CREATE TABLE plats (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    type ENUM('Entrée', 'Plat principal', 'Dessert') NOT NULL,
    cuisinier_id INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (cuisinier_id) REFERENCES cuisiniers(id) ON DELETE SET NULL
);

-- Insertion de plats de test
INSERT INTO plats (nom, description, prix, type, cuisinier_id) VALUES
('Bœuf bourguignon', 'Plat traditionnel français à base de bœuf mijoté au vin rouge', 24.90, 'Plat principal', 1),
('Pizza Margherita', 'Pizza traditionnelle italienne avec tomate, mozzarella et basilic', 12.50, 'Plat principal', 2),
('Pad Thai', 'Nouilles sautées thaïlandaises avec poulet et légumes', 18.75, 'Plat principal', 3),
('Tarte Tatin', 'Dessert français aux pommes caramélisées', 8.90, 'Dessert', 1),
('Tiramisu', 'Dessert italien au café et mascarpone', 7.50, 'Dessert', 2);

-- Création de la table des commandes
CREATE TABLE commandes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    client_id INT NOT NULL,
    date_commande TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    statut ENUM('En attente', 'En préparation', 'Prête', 'Livrée', 'Annulée') NOT NULL DEFAULT 'En attente',
    total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (client_id) REFERENCES clients(id) ON DELETE CASCADE
);

-- Création de la table des détails de commande
CREATE TABLE details_commande (
    id INT AUTO_INCREMENT PRIMARY KEY,
    commande_id INT NOT NULL,
    plat_id INT NOT NULL,
    quantite INT NOT NULL,
    prix_unitaire DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (commande_id) REFERENCES commandes(id) ON DELETE CASCADE,
    FOREIGN KEY (plat_id) REFERENCES plats(id) ON DELETE CASCADE
);

-- Création de la table des stations de métro
CREATE TABLE stations_metro (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    latitude DECIMAL(10,8) NOT NULL,
    longitude DECIMAL(11,8) NOT NULL,
    ligne VARCHAR(50) NOT NULL
);

-- Insertion de quelques stations de métro de test
INSERT INTO stations_metro (nom, latitude, longitude, ligne) VALUES
('Châtelet', 48.8584, 2.3476, '1,4,7,11,14'),
('Gare de Lyon', 48.8447, 2.3733, '1,14'),
('Opéra', 48.8719, 2.3317, '3,7,8'),
('Bastille', 48.8534, 2.3688, '1,5,8'),
('Montparnasse', 48.8422, 2.3215, '4,6,12,13');

-- Création de la table des connexions entre stations
CREATE TABLE connexions_metro (
    id INT AUTO_INCREMENT PRIMARY KEY,
    station_depart_id INT NOT NULL,
    station_arrivee_id INT NOT NULL,
    distance DECIMAL(10,2) NOT NULL,
    duree INT NOT NULL, -- en secondes
    FOREIGN KEY (station_depart_id) REFERENCES stations_metro(id) ON DELETE CASCADE,
    FOREIGN KEY (station_arrivee_id) REFERENCES stations_metro(id) ON DELETE CASCADE
);

-- Insertion de quelques connexions de test
INSERT INTO connexions_metro (station_depart_id, station_arrivee_id, distance, duree) VALUES
(1, 2, 1.2, 180), -- Châtelet -> Gare de Lyon
(1, 3, 0.8, 120), -- Châtelet -> Opéra
(1, 4, 0.9, 150), -- Châtelet -> Bastille
(2, 5, 1.5, 240), -- Gare de Lyon -> Montparnasse
(3, 5, 1.3, 210); -- Opéra -> Montparnasse

-- Création des index pour améliorer les performances
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_cuisiniers_email ON cuisiniers(email);
CREATE INDEX idx_clients_email ON clients(email);
CREATE INDEX idx_plats_type ON plats(type);
CREATE INDEX idx_commandes_date ON commandes(date_commande);
CREATE INDEX idx_stations_metro_nom ON stations_metro(nom);
CREATE INDEX idx_connexions_stations ON connexions_metro(station_depart_id, station_arrivee_id);

-- Création des triggers pour la mise à jour automatique des timestamps
DELIMITER //
CREATE TRIGGER before_update_cuisiniers
BEFORE UPDATE ON cuisiniers
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP;
END//

CREATE TRIGGER before_update_clients
BEFORE UPDATE ON clients
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP;
END//

CREATE TRIGGER before_update_plats
BEFORE UPDATE ON plats
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP;
END//
DELIMITER ; 
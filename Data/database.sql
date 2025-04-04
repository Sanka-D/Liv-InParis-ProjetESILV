-- Active: 1743777901514@@127.0.0.1@3306@livinparis
-- Création de la base de données
CREATE DATABASE IF NOT EXISTS livinparis;
USE livinparis;

-- Création de la table des utilisateurs
CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role ENUM('Admin', 'Manager', 'User') NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insertion d'utilisateurs de test
INSERT INTO users (username, password, role, email) VALUES
('admin', 'admin123', 'Admin', 'admin@livinparis.com'),
('manager', 'manager123', 'Manager', 'manager@livinparis.com'),
('user', 'user123', 'User', 'user@livinparis.com');

-- Création de la table des droits
CREATE TABLE IF NOT EXISTS permissions (
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
CREATE TABLE IF NOT EXISTS cuisiniers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    specialite VARCHAR(100) NOT NULL,
    experience INT NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telephone VARCHAR(20) NOT NULL
);

-- Insertion de cuisiniers de test
INSERT INTO cuisiniers (nom, prenom, specialite, experience, email, telephone) VALUES
('Dupont', 'Jean', 'Cuisine française', 10, 'jean.dupont@cuisine.com', '0612345678'),
('Martin', 'Sophie', 'Cuisine italienne', 8, 'sophie.martin@cuisine.com', '0623456789'),
('Bernard', 'Pierre', 'Cuisine asiatique', 12, 'pierre.bernard@cuisine.com', '0634567890');

-- Création de la table des clients
CREATE TABLE IF NOT EXISTS clients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    adresse VARCHAR(200) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telephone VARCHAR(20) NOT NULL,
    preferences VARCHAR(200)
);

-- Insertion de clients de test
INSERT INTO clients (nom, prenom, adresse, email, telephone, preferences) VALUES
('Leroy', 'Marie', '15 rue de Paris, 75001', 'marie.leroy@client.com', '0645678901', 'Végétarien'),
('Petit', 'Thomas', '22 avenue des Champs, 75008', 'thomas.petit@client.com', '0656789012', 'Sans gluten'),
('Moreau', 'Julie', '8 boulevard Saint-Germain, 75006', 'julie.moreau@client.com', '0667890123', 'Halal');

-- Création de la table des plats
CREATE TABLE IF NOT EXISTS plats (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    type ENUM('Entrée', 'Plat principal', 'Dessert') NOT NULL,
    cuisinier_id INT,
    FOREIGN KEY (cuisinier_id) REFERENCES cuisiniers(id)
);

-- Insertion de plats de test
INSERT INTO plats (nom, description, prix, type, cuisinier_id) VALUES
('Bœuf bourguignon', 'Plat traditionnel français à base de bœuf mijoté au vin rouge', 24.90, 'Plat principal', 1),
('Pizza Margherita', 'Pizza traditionnelle italienne avec tomate, mozzarella et basilic', 12.50, 'Plat principal', 2),
('Pad Thai', 'Nouilles sautées thaïlandaises avec poulet et légumes', 18.75, 'Plat principal', 3),
('Tarte Tatin', 'Dessert français aux pommes caramélisées', 8.90, 'Dessert', 1),
('Tiramisu', 'Dessert italien au café et mascarpone', 7.50, 'Dessert', 2); 
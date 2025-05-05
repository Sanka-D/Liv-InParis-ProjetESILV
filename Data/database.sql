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

-- Ajout de nouveaux clients
INSERT INTO clients (nom, prenom, adresse, email, telephone, preferences) VALUES
('Dubois', 'Lucas', '45 rue de Rivoli, 75004', 'lucas.dubois@email.com', '0678901234', 'Végétarien'),
('Lefebvre', 'Emma', '12 rue du Commerce, 75015', 'emma.lefebvre@email.com', '0689012345', 'Sans gluten'),
('Garcia', 'Antoine', '78 avenue des Ternes, 75017', 'antoine.garcia@email.com', '0690123456', 'Halal'),
('Roux', 'Camille', '23 rue de la Paix, 75002', 'camille.roux@email.com', '0601234567', 'Végan'),
('Fournier', 'Hugo', '56 boulevard Haussmann, 75009', 'hugo.fournier@email.com', '0612345678', 'Sans lactose'),
('Morel', 'Léa', '34 rue de la Pompe, 75016', 'lea.morel@email.com', '0623456789', 'Sans gluten'),
('Girard', 'Nathan', '89 rue de la Roquette, 75011', 'nathan.girard@email.com', '0634567890', 'Végétarien'),
('Bonnet', 'Chloé', '67 rue de Passy, 75016', 'chloe.bonnet@email.com', '0645678901', 'Halal'),
('Dupuis', 'Maxime', '45 rue de la Convention, 75015', 'maxime.dupuis@email.com', '0656789012', 'Sans gluten'),
('Lambert', 'Sarah', '23 rue de la Fayette, 75009', 'sarah.lambert@email.com', '0667890123', 'Végan');

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

-- Ajout de nouveaux plats
INSERT INTO plats (nom, description, prix, type, cuisinier_id) VALUES
-- Entrées
('Salade de chèvre chaud', 'Salade verte avec chèvre chaud sur toast', 8.90, 'Entrée', 1),
('Bruschetta', 'Tartines grillées aux tomates et basilic', 7.50, 'Entrée', 2),
('Nems au poulet', 'Rouleaux de printemps frits au poulet', 6.90, 'Entrée', 3),
('Soupe à l''oignon', 'Soupe traditionnelle française gratinée', 9.50, 'Entrée', 1),
('Carpaccio de bœuf', 'Tranches fines de bœuf avec huile d''olive et parmesan', 12.90, 'Entrée', 2),

-- Plats principaux
('Coq au vin', 'Poulet mijoté au vin rouge et légumes', 22.90, 'Plat principal', 1),
('Risotto aux champignons', 'Riz crémeux aux champignons et parmesan', 18.50, 'Plat principal', 2),
('Poulet Kung Pao', 'Poulet épicé aux arachides et légumes', 19.90, 'Plat principal', 3),
('Filet de bar', 'Filet de bar poêlé avec légumes de saison', 24.90, 'Plat principal', 1),
('Lasagne bolognaise', 'Pâtes feuilletées à la sauce bolognaise', 16.90, 'Plat principal', 2),
('Canard laqué', 'Canard laqué avec sauce hoisin', 23.90, 'Plat principal', 3),
('Steak frites', 'Steak de bœuf avec frites maison', 25.90, 'Plat principal', 1),
('Raviolis aux épinards', 'Raviolis frais aux épinards et ricotta', 17.90, 'Plat principal', 2),

-- Desserts
('Crème brûlée', 'Crème vanille caramélisée', 7.90, 'Dessert', 1),
('Panna cotta', 'Crème italienne avec coulis de fruits rouges', 6.90, 'Dessert', 2),
('Mochi glacé', 'Pâtisserie japonaise glacée', 5.90, 'Dessert', 3),
('Profiteroles', 'Choux fourrés à la crème avec sauce chocolat', 8.90, 'Dessert', 1),
('Tiramisu aux fruits', 'Tiramisu revisité avec fruits de saison', 7.50, 'Dessert', 2),
('Glace au thé vert', 'Glace au matcha avec pâte de haricot rouge', 6.50, 'Dessert', 3),
('Tarte au citron', 'Tarte au citron meringuée', 7.90, 'Dessert', 1);

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

-- Statistiques des plats
SELECT 
    'Statistiques des plats' as 'Catégorie',
    COUNT(*) as 'Nombre total de plats',
    ROUND(AVG(prix), 2) as 'Prix moyen',
    MIN(prix) as 'Prix minimum',
    MAX(prix) as 'Prix maximum',
    SUM(CASE WHEN type = 'Entrée' THEN 1 ELSE 0 END) as 'Nombre d''entrées',
    SUM(CASE WHEN type = 'Plat principal' THEN 1 ELSE 0 END) as 'Nombre de plats principaux',
    SUM(CASE WHEN type = 'Dessert' THEN 1 ELSE 0 END) as 'Nombre de desserts'
FROM plats;

-- Statistiques par cuisinier
SELECT 
    c.nom as 'Nom cuisinier',
    c.prenom as 'Prénom cuisinier',
    c.specialite as 'Spécialité',
    COUNT(p.id) as 'Nombre de plats',
    ROUND(AVG(p.prix), 2) as 'Prix moyen des plats',
    MIN(p.prix) as 'Prix minimum',
    MAX(p.prix) as 'Prix maximum'
FROM cuisiniers c
LEFT JOIN plats p ON c.id = p.cuisinier_id
GROUP BY c.id
ORDER BY COUNT(p.id) DESC;

-- Statistiques des plats par type
SELECT 
    type as 'Type de plat',
    COUNT(*) as 'Nombre de plats',
    ROUND(AVG(prix), 2) as 'Prix moyen',
    MIN(prix) as 'Prix minimum',
    MAX(prix) as 'Prix maximum'
FROM plats
GROUP BY type
ORDER BY type;

-- Statistiques des plats par cuisinier et type
SELECT 
    c.nom as 'Nom cuisinier',
    c.prenom as 'Prénom cuisinier',
    p.type as 'Type de plat',
    COUNT(*) as 'Nombre de plats',
    ROUND(AVG(p.prix), 2) as 'Prix moyen'
FROM cuisiniers c
JOIN plats p ON c.id = p.cuisinier_id
GROUP BY c.id, p.type
ORDER BY c.nom, p.type;

-- Statistiques des clients
SELECT 
    COUNT(*) as 'Nombre total de clients',
    COUNT(DISTINCT preferences) as 'Nombre de préférences différentes',
    COUNT(CASE WHEN preferences LIKE '%Végétarien%' OR preferences LIKE '%Végan%' THEN 1 END) as 'Clients végétariens/végans',
    COUNT(CASE WHEN preferences LIKE '%Sans gluten%' THEN 1 END) as 'Clients sans gluten',
    COUNT(CASE WHEN preferences LIKE '%Halal%' THEN 1 END) as 'Clients halal'
FROM clients;

-- Statistiques des préférences alimentaires
SELECT 
    preferences as 'Préférence alimentaire',
    COUNT(*) as 'Nombre de clients'
FROM clients
GROUP BY preferences
ORDER BY COUNT(*) DESC;

-- Statistiques des plats par gamme de prix
SELECT 
    CASE 
        WHEN prix < 10 THEN 'Moins de 10€'
        WHEN prix < 15 THEN '10-15€'
        WHEN prix < 20 THEN '15-20€'
        WHEN prix < 25 THEN '20-25€'
        ELSE 'Plus de 25€'
    END as 'Gamme de prix',
    COUNT(*) as 'Nombre de plats',
    ROUND(AVG(prix), 2) as 'Prix moyen'
FROM plats
GROUP BY 
    CASE 
        WHEN prix < 10 THEN 'Moins de 10€'
        WHEN prix < 15 THEN '10-15€'
        WHEN prix < 20 THEN '15-20€'
        WHEN prix < 25 THEN '20-25€'
        ELSE 'Plus de 25€'
    END
ORDER BY MIN(prix);

-- Statistiques des plats par arrondissement (basé sur l'adresse des clients)
SELECT 
    SUBSTRING_INDEX(SUBSTRING_INDEX(c.adresse, ', ', -1), ' ', 1) as 'Arrondissement',
    COUNT(DISTINCT c.id) as 'Nombre de clients',
    COUNT(DISTINCT p.id) as 'Nombre de plats disponibles'
FROM clients c
CROSS JOIN plats p
GROUP BY SUBSTRING_INDEX(SUBSTRING_INDEX(c.adresse, ', ', -1), ' ', 1)
ORDER BY COUNT(DISTINCT c.id) DESC;

-- Statistiques des plats les plus chers par type
SELECT 
    p.type as 'Type de plat',
    p.nom as 'Nom du plat',
    p.prix as 'Prix',
    c.nom as 'Nom cuisinier',
    c.prenom as 'Prénom cuisinier'
FROM plats p
JOIN cuisiniers c ON p.cuisinier_id = c.id
WHERE (p.type, p.prix) IN (
    SELECT type, MAX(prix)
    FROM plats
    GROUP BY type
)
ORDER BY p.prix DESC;

-- Statistiques des plats les moins chers par type
SELECT 
    p.type as 'Type de plat',
    p.nom as 'Nom du plat',
    p.prix as 'Prix',
    c.nom as 'Nom cuisinier',
    c.prenom as 'Prénom cuisinier'
FROM plats p
JOIN cuisiniers c ON p.cuisinier_id = c.id
WHERE (p.type, p.prix) IN (
    SELECT type, MIN(prix)
    FROM plats
    GROUP BY type
)
ORDER BY p.prix; 
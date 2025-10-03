-- DENTISTA360 Database Schema
-- Criar database primeiro: CREATE DATABASE DENTISTA360;
-- USE DENTISTA360;

-- Tabela Endereço
CREATE TABLE Endereco (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Logradouro NVARCHAR(200) NOT NULL,
    Numero NVARCHAR(20) NOT NULL,
    Bairro NVARCHAR(100) NOT NULL,
    Cidade NVARCHAR(100) NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    CEP NVARCHAR(10) NOT NULL,
    Complemento NVARCHAR(200) NULL
);

-- Tabela Grupo
CREATE TABLE Grupo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DescricaoGrupo NVARCHAR(100) NOT NULL
);

-- Tabela Clínica
CREATE TABLE Clinica (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NomeFantasia NVARCHAR(200) NOT NULL,
    RazaoSocial NVARCHAR(200) NOT NULL,
    CNPJ NVARCHAR(18) NOT NULL,
    EnderecoId INT NOT NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(200) NULL,
    NomeResponsavel NVARCHAR(200) NOT NULL,
    CONSTRAINT FK_Clinica_Endereco FOREIGN KEY (EnderecoId) REFERENCES Endereco(Id)
);

-- Tabela User
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    EnderecoId INT NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Cargo NVARCHAR(100) NULL,
    Senha NVARCHAR(255) NOT NULL,
    CPF NVARCHAR(14) NOT NULL UNIQUE,
    CONSTRAINT FK_User_Endereco FOREIGN KEY (EnderecoId) REFERENCES Endereco(Id)
);

-- Tabela user_clinic (relacionamento usuário-clínica)
CREATE TABLE user_clinic (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    clinic_id INT NOT NULL,
    group_type NVARCHAR(50) NOT NULL, -- employee, director, doctor
    CONSTRAINT FK_UserClinic_User FOREIGN KEY (user_id) REFERENCES [User](Id),
    CONSTRAINT FK_UserClinic_Clinic FOREIGN KEY (clinic_id) REFERENCES Clinica(Id),
    CONSTRAINT UQ_UserClinic UNIQUE (user_id, clinic_id)
);

-- Inserir dados de exemplo para grupos
INSERT INTO Grupo (DescricaoGrupo) VALUES 
('SUPER ADMIN'),
('ADMIN'),
('GERENTE'),
('FINANCEIRO'),
('RECEPCAO'),
('ESTOQUE'),
('MEDICO');

-- Inserir endereço de exemplo
INSERT INTO Endereco (Logradouro, Numero, Bairro, Cidade, Estado, CEP, Complemento) VALUES 
('Rua das Flores', '123', 'Centro', 'São Paulo', 'SP', '01234-567', 'Sala 101');

-- Inserir clínica de exemplo
INSERT INTO Clinica (NomeFantasia, RazaoSocial, CNPJ, EnderecoId, Phone, Email, NomeResponsavel) VALUES 
('Clinica Lima', 'Clinica Lima Ltda', '12.345.678/0001-90', 1, '(11) 99999-9999', 'contato@clinicalima.com', 'Dr. João Lima');

-- Inserir usuário de exemplo (senha: 123456)
INSERT INTO [User] (Nome, EnderecoId, Phone, Email, Cargo, Senha, CPF) VALUES 
('Henrique Lima', NULL, '(11) 88888-8888', 'henrique@clinicalima.com', 'Gerente', '$2a$11$8GZmOV8Xgv3qSGKz8Ys9Hu.CQr3/0LGJb9Ue7oQz8xGz7Ys9Hu.CQ', '123.456.789-00');

-- Relacionar usuário com clínica
INSERT INTO user_clinic (user_id, clinic_id, group_type) VALUES 
(1, 1, 'director');

-- Índices para melhor performance
CREATE INDEX IX_User_Email ON [User](Email);
CREATE INDEX IX_User_CPF ON [User](CPF);
CREATE INDEX IX_UserClinic_UserId ON user_clinic(user_id);
CREATE INDEX IX_UserClinic_ClinicId ON user_clinic(clinic_id);

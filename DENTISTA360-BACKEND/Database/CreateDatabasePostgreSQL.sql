-- DENTISTA360 Database Schema para PostgreSQL
-- Criar database primeiro: CREATE DATABASE DENTISTA360;

-- Tabela Endereço
CREATE TABLE IF NOT EXISTS "Endereco" (
    "Id" SERIAL PRIMARY KEY,
    "Logradouro" VARCHAR(200) NOT NULL,
    "Numero" VARCHAR(20) NOT NULL,
    "Bairro" VARCHAR(100) NOT NULL,
    "Cidade" VARCHAR(100) NOT NULL,
    "Estado" VARCHAR(50) NOT NULL,
    "CEP" VARCHAR(10) NOT NULL,
    "Complemento" VARCHAR(200) NULL
);

-- Tabela Grupo
CREATE TABLE IF NOT EXISTS "Grupo" (
    "Id" SERIAL PRIMARY KEY,
    "DescricaoGrupo" VARCHAR(100) NOT NULL
);

-- Tabela Clínica
CREATE TABLE IF NOT EXISTS "Clinica" (
    "Id" SERIAL PRIMARY KEY,
    "NomeFantasia" VARCHAR(200) NOT NULL,
    "RazaoSocial" VARCHAR(200) NOT NULL,
    "CNPJ" VARCHAR(18) NOT NULL,
    "EnderecoId" INTEGER NOT NULL,
    "Phone" VARCHAR(50) NULL,
    "Email" VARCHAR(200) NULL,
    "NomeResponsavel" VARCHAR(200) NOT NULL,
    CONSTRAINT "FK_Clinica_Endereco" FOREIGN KEY ("EnderecoId") REFERENCES "Endereco"("Id")
);

-- Tabela User
CREATE TABLE IF NOT EXISTS "User" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(200) NOT NULL,
    "EnderecoId" INTEGER NULL,
    "Phone" VARCHAR(50) NULL,
    "Email" VARCHAR(200) NOT NULL UNIQUE,
    "Cargo" VARCHAR(100) NULL,
    "Senha" VARCHAR(255) NOT NULL,
    "CPF" VARCHAR(14) NOT NULL UNIQUE,
    CONSTRAINT "FK_User_Endereco" FOREIGN KEY ("EnderecoId") REFERENCES "Endereco"("Id")
);

-- Tabela user_clinic (relacionamento usuário-clínica)
CREATE TABLE IF NOT EXISTS user_clinic (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    clinic_id INTEGER NOT NULL,
    group_type VARCHAR(50) NOT NULL, -- employee, director, doctor
    CONSTRAINT "FK_UserClinic_User" FOREIGN KEY (user_id) REFERENCES "User"("Id"),
    CONSTRAINT "FK_UserClinic_Clinic" FOREIGN KEY (clinic_id) REFERENCES "Clinica"("Id"),
    CONSTRAINT "UQ_UserClinic" UNIQUE (user_id, clinic_id)
);

-- Inserir dados de exemplo para grupos
INSERT INTO "Grupo" ("DescricaoGrupo") VALUES 
('SUPER ADMIN'),
('ADMIN'),
('GERENTE'),
('FINANCEIRO'),
('RECEPCAO'),
('ESTOQUE'),
('MEDICO')
ON CONFLICT DO NOTHING;

-- Inserir endereço de exemplo
INSERT INTO "Endereco" ("Logradouro", "Numero", "Bairro", "Cidade", "Estado", "CEP", "Complemento") VALUES 
('Rua das Flores', '123', 'Centro', 'São Paulo', 'SP', '01234-567', 'Sala 101')
ON CONFLICT DO NOTHING;

-- Inserir clínica de exemplo
INSERT INTO "Clinica" ("NomeFantasia", "RazaoSocial", "CNPJ", "EnderecoId", "Phone", "Email", "NomeResponsavel") VALUES 
('Clinica Lima', 'Clinica Lima Ltda', '12.345.678/0001-90', 1, '(11) 99999-9999', 'contato@clinicalima.com', 'Dr. João Lima')
ON CONFLICT DO NOTHING;

-- Inserir usuário de exemplo (senha: 123456)
INSERT INTO "User" ("Nome", "EnderecoId", "Phone", "Email", "Cargo", "Senha", "CPF") VALUES 
('Henrique Lima', NULL, '(11) 88888-8888', 'henrique@clinicalima.com', 'Gerente', '$2a$11$8GZmOV8Xgv3qSGKz8Ys9Hu.CQr3/0LGJb9Ue7oQz8xGz7Ys9Hu.CQ', '123.456.789-00')
ON CONFLICT ("Email") DO NOTHING;

-- Relacionar usuário com clínica
INSERT INTO user_clinic (user_id, clinic_id, group_type) VALUES 
(1, 1, 'director')
ON CONFLICT (user_id, clinic_id) DO NOTHING;

-- Índices para melhor performance
CREATE INDEX IF NOT EXISTS "IX_User_Email" ON "User"("Email");
CREATE INDEX IF NOT EXISTS "IX_User_CPF" ON "User"("CPF");
CREATE INDEX IF NOT EXISTS "IX_UserClinic_UserId" ON user_clinic(user_id);
CREATE INDEX IF NOT EXISTS "IX_UserClinic_ClinicId" ON user_clinic(clinic_id);

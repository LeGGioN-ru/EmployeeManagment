create database EmpControl
USE EmpControl

Create table [Employee]
(
	[Employee_id] Integer Identity NOT NULL,
	[Post_id] Integer NOT NULL,
	[Employee_name] Nvarchar(100) NOT NULL,
	[Employee_surname] Nvarchar(100) NOT NULL,
	[Employee_patronymic] Nvarchar(100) NULL,
	[Phone_number_emp] Nvarchar(20) NOT NULL,
	[Birthday] Datetime NOT NULL,
	[Address] Nvarchar(140) NOT NULL,
Primary Key ([Employee_id])
) 
go

Create table [Post]
(
	[Post_id] Integer Identity NOT NULL,
	[Post_name] Nvarchar(100) NOT NULL,
	[Post_discription] Nvarchar(500) NULL,
	[Salary_id] Integer NOT NULL,
Primary Key ([Post_id])
) 
go

Create table [Salary]
(
	[Salary_id] Integer Identity NOT NULL,
	[Salary_volume] Integer NOT NULL,
Primary Key ([Salary_id])
) 
go

Create table [Passport_data]
(
	[Employee_id] Integer NOT NULL,
	[Passport_serial] Nvarchar(20) NOT NULL,
	[Passport_number] Nvarchar(12) NOT NULL,
Primary Key ([Employee_id])
) 
go

Create table [Emp_discription]
(
	[Employee_id] Integer NOT NULL,
	[Emp_type_id] Integer NOT NULL,
	[Date_of_employment] Datetime NOT NULL,
Primary Key ([Employee_id])
) 
go

Create table [Emp_type]
(
	[Emp_type_id] Integer Identity NOT NULL,
	[Type_name] Nvarchar(100) NOT NULL,
Primary Key ([Emp_type_id])
) 
go


Alter table [Passport_data] add  foreign key([Employee_id]) references [Employee] ([Employee_id])  on update no action on delete no action 
go
Alter table [Emp_discription] add  foreign key([Employee_id]) references [Employee] ([Employee_id])  on update no action on delete no action 
go
Alter table [Employee] add  foreign key([Post_id]) references [Post] ([Post_id])  on update no action on delete no action 
go
Alter table [Post] add  foreign key([Salary_id]) references [Salary] ([Salary_id])  on update no action on delete no action 
go
Alter table [Emp_discription] add  foreign key([Emp_type_id]) references [Emp_type] ([Emp_type_id])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


INSERT INTO Salary
VALUES
(100000),
(70000),
(50000),
(45000),
(30000),
(65000),
(60000),
(80000),
(40000),
(150000),
(120000)

INSERT INTO Post (Post_name,Salary_id,Post_discription)
VALUES
('���������',4,'��������� �������� ����� �� ��������� ����������'),
('������� ���������',2,'�������� �� ����������'),
('��������',3,'��������� ����������� ������ �� ��������'),
('������� ��������',1,'��������� ���������������� �������� ��� ��������'),
('��������� �������������',8,'������� � ��������� ��������� �����'),
('�������� �����������',10,'������������� �� ��� ����� �����������'),
('��� ��������� �����������',11,'�������� ��������� � �������� � ��������������� ��������'),
('������������� ��������',5,'������ �� �������� � ������� ����������')

INSERT INTO Emp_type
VALUES
('���������� ���������'),
('��������� ���������'),
('�����������')

INSERT INTO Employee (Employee_surname,Employee_name,Employee_patronymic,Birthday,[Address],Phone_number_emp,Post_id)
VALUES 
('��������','�������','����������','07.01.1981','������, �. ������, ��������� ��., �. 12 ��.118','7(495)899-41-76',1),
('��������','�����','�����������','10.12.1986','������, �. �������, ������������ ���., �. 10 ��.175','7(495)663-30-05',2),
('������','�������','���������','02.05.1983','������, �. ��������������, ������� ��., �. 15 ��.37','7(495)278-71-43',3),
('�������','�����','������������','11.10.1976','������, �. �������, ������� ���., �. 25 ��.122','7(495)697-33-82',4),
('�����������','�����','��������������','09.09.1992','������, �. ���������, ����� ��., �. 23 ��.41','7(495)816-99-33',5),
('�������','�����','�����������','08.05.1967','������, �. ���������, ������� ��., �. 10 ��.106','7(495)969-06-49',6),
('�������','�������','����������','07.12.1995','������, �. ������, ��������� ��., �. 22 ��.3','7(495)277-31-03',7),
('���������','�������','�����������','01.09.1984','������, �. ���������, ���������� ���., �. 21 ��.21','7(495)011-39-45',8)

INSERT INTO Emp_discription (Employee_id,Emp_type_id,Date_of_employment)
VALUES
(1,1,'10.10.2018'),
(2,1,'09.09.2018'),
(3,2,'08.08.2019'),
(4,1,'07.07.2018'),
(5,3,'06.06.2017'),
(6,1,'05.05.2020'),
(7,2,'10.10.2022'),
(8,3,'11.11.2016')

INSERT INTO Passport_data (Employee_id,Passport_serial,Passport_number)
VALUES
(1,'4655','419189'),
(2,'4252','157538'),
(3,'4625','437342'),
(4,'4615','815018'),
(5,'4290','187589'),
(6,'4827','949629'),
(7,'4162','999113'),
(8,'4348','161680')

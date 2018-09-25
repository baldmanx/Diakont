CREATE TABLE Jobs(job_id UNIQUEIDENTIFIER PRIMARY KEY,
                  job_name NVARCHAR(50) NOT NULL,
				  jdate_from DATE,
				  fee MONEY);

CREATE TABLE Departments(department_id UNIQUEIDENTIFIER PRIMARY KEY,
                         department_name NVARCHAR(50) NOT NULL,
		                 ddate_from DATE,
				         workers_amount INT);

CREATE TABLE Reports(report_id UNIQUEIDENTIFIER PRIMARY KEY,
                     j_id UNIQUEIDENTIFIER NOT NULL,
					 d_id UNIQUEIDENTIFIER NOT NULL,
                     date_from DATE NOT NULL,
				     date_to DATE NOT NULL,
				     monthly_pay MONEY NOT NULL,
					 FOREIGN KEY(j_id) REFERENCES Jobs(job_id),
					 FOREIGN KEY(d_id) REFERENCES Departments(department_id));

INSERT INTO Jobs(job_id, job_name) VALUES (NEWID(), 'Инженер-конструктор');
INSERT INTO Jobs(job_id, job_name) VALUES (NEWID(), 'Инженер-технолог');

INSERT INTO Departments(department_id, department_name) VALUES (NEWID(), 'Конструкторское бюро');
INSERT INTO Departments(department_id, department_name) VALUES (NEWID(), 'Технологический отдел');
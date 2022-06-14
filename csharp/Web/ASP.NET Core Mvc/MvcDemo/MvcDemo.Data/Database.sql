create table Tasks
(
	Id int identity not null primary key,
	UserId int not null,
	Title varchar(20) not null,
	Description varchar(255) not null,
	IsCompleted bit not null,
	CreatedAtUtc datetime not null,
	CompletedAtUtc datetime
);

create table Users
(
	Id int identity not null primary key,
	Name varchar(40) not null
);

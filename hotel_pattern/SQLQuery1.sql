create table roles (
	id int primary key identity(1,1) ,
	role_name varchar(40),
	);
create table users (
	id int primary key identity(1,1),
	login varchar(100),
	password varchar(50),
	role_id int,
	block_status varchar(40)
	foreign key (role_id) references roles(id),
	);
create table gender(
	id int primary key identity(1,1), 
	gender_name varchar (10),

	);
create table guests (
	id int primary key identity(1,1),
	surname varchar(60),
	name varchar(60),
	pastname varchar(60),
	phone varchar (15),
	passport varchar(10),
	gender_id int,
	foreign key (gender_id) references gender(id),
	);
create table staff (
	id int primary key identity(1,1),
	surname varchar(60),
	name varchar(60),
	pastname varchar(60),
	position varchar (30),
	);
create table room_statuses (
	id int primary key identity(1,1),
	status_name varchar(60),
	
	);
create table rooms (
	id int primary key identity(1,1),
	category varchar(100),
	price_per_day int,
	status_id int,
	foreign key (status_id) references room_statuses(id),
	);
create table bookings (
	id int primary key identity(1,1),
	guest_id int,
	room_id int, 
	check_in date,
	check_out date,
	cost int,
	foreign key (room_id) references rooms(id),
	foreign key (guest_id) references guests(id),
	);
create table clean_statuses (
	id int primary key identity(1,1),
	status_name varchar(60),
	
	);
create table cleaning_shedule (
	id int primary key identity(1,1),
	staff_id int,
	room_id int, 
	status_id int,
	foreign key (status_id) references clean_statuses(id),
	foreign key (room_id) references rooms(id),
	foreign key (staff_id) references staff(id),
	);
create table payments (
	id int primary key identity(1,1),
	booking_id int,
	foreign key (booking_id) references bookings(id),
	amount int,
	type varchar (50),
	pay_date date
	);
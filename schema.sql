use master;
go
create database nongsan_db;
go

use nongsan_db;
go
create table dh_user(
    id int identity primary key,
    username varchar(50),
    password varchar(100),
    name nvarchar(255),
    email varchar(50),
    phone varchar(12),
    address nvarchar(255),
    role int,
    created bigint,
    updated bigint
);
go

create table dh_order(
    id int identity primary key,
    user_id int,
    address nvarchar(255),
    name nvarchar(255),
    email varchar(50),
    code_name varchar(10),
    status int,
    created bigint,
    updated bigint
);
go
create table dh_cart(
    id int identity primary key,
    user_id int,
    quantity int,
    price bigint,
    product_id int,
    created bigint,
    updated bigint
);
go
create table dh_blog(
    id int identity primary key,
    thumbnail varchar(max),
    content nvarchar(max),
    created bigint,
    updated bigint
);
go
create table dh_category(
    id int identity primary key,
    name nvarchar(255),
    description nvarchar(max),
    created bigint,
    updated bigint
);
go
create table dh_product(
   id int identity primary key,
   name nvarchar(100),
   description nvarchar(max),
   price bigint,
   quantity int,
   category_id int,
   avatar nvarchar(max),
   created bigint,
   updated bigint
);
go
create table dh_order_product(
    id int identity primary key,
    order_id int,
    product_id int,
    price bigint,
    quantity int
);
go
alter table dh_order add foreign key(user_id) references dh_user(id);
go 
alter table dh_cart add foreign key(user_id) references dh_user(id);
alter table dh_cart add foreign key(product_id) references dh_product(id);
go
alter table dh_order_product add foreign key(order_id) references dh_order(id);
alter table dh_order_product add foreign key(product_id) references dh_product(id);
go
alter table dh_product add foreign key (category_id) references dh_category(id);
go
alter table dh_category add avatar varchar(max)

create table app_user
(
id integer primary key generated always as identity,
created_on timestamp
);

create table website_host
(
id integer primary key generated always as identity,
dns_name varchar(254) not null unique
);

create table website_host_comment
(
id integer primary key generated always as identity,
content varchar(512),
app_user_id integer not null references app_user(id),
website_host_id integer not null references website_host(id),
created_on timestamp not null
);

create table website_host_vote
(
id integer primary key generated always as identity,
app_user_id integer not null references app_user,
website_host_id integer not null references website_host(id),
vote character(1) not null
);

create index on website_host(dns_name);
create index on website_host_comment(website_host_id);
create index on website_host_vote(website_host_id);
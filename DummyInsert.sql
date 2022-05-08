use RopeyDVD;
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Friction', 0);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Action', 0);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Adventure', 0);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Crime', 1);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Adult', 1);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Comedy', 0);
insert into DVDCategories(CategoryDescription, AgeRestricted) values('Biography', 0);



insert into Actors(ActorSurname, ActorFirstName) values('Hamal', 'Rajesh');
insert into Actors(ActorSurname, ActorFirstName) values('Upreti', 'Nikhil');
insert into Actors(ActorSurname, ActorFirstName) values('K.C.', 'Bhuban');
insert into Actors(ActorSurname, ActorFirstName) values('K.C.', 'Anmol');
insert into Actors(ActorSurname, ActorFirstName) values('Hang Rai', 'Daya');
insert into Actors(ActorSurname, ActorFirstName) values('Malla', 'Saugat');
insert into Actors(ActorSurname, ActorFirstName) values('Di caprip', 'Leo');



insert into Studios(StudioName) values('Pixar');
insert into Studios(StudioName) values('Hilights Nepal');
insert into Studios(StudioName) values('Disney');
insert into Studios(StudioName) values('Sony');
insert into Studios(StudioName) values('Marval');
insert into Studios(StudioName) values('DC');



insert into Producers(ProducerName) values('Kanye West');
insert into Producers(ProducerName) values('Quintin Tarantino');
insert into Producers(ProducerName) values('Martin Scorsese');
insert into Producers(ProducerName) values('Harmony');
insert into Producers(ProducerName) values('Nischal bashnet');

insert into DVDTitles(DVDTitles, DateReleased, StandardCharge, PenaltyCharge, CategoryNumber, StudioNumber, ProducerNumber) values('Kabadi kabadi', '2018-06-23', 50,  100, 6, 2, 3);  
insert into DVDTitles(DVDTitles, DateReleased, StandardCharge, PenaltyCharge, CategoryNumber, StudioNumber, ProducerNumber) values('Spiderman',     '2013-07-03', 100, 200, 2, 2, 2);  
insert into DVDTitles(DVDTitles, DateReleased, StandardCharge, PenaltyCharge, CategoryNumber, StudioNumber, ProducerNumber) values('Inception',     '2011-06-23', 150, 200, 3, 3, 5);  
insert into DVDTitles(DVDTitles, DateReleased, StandardCharge, PenaltyCharge, CategoryNumber, StudioNumber, ProducerNumber) values('Into the wild', '2012-06-23', 90,  100, 1, 1, 1);  
insert into DVDTitles(DVDTitles, DateReleased, StandardCharge, PenaltyCharge, CategoryNumber, StudioNumber, ProducerNumber) values('Raman Ragav',   '2015-05-23', 500, 600, 6, 2, 3);  

insert into DVDCopies(DatePurchased, DVDNumber) values('2015-05-23', 1);
insert into DVDCopies(DatePurchased, DVDNumber) values('2014-03-17', 3);
insert into DVDCopies(DatePurchased, DVDNumber) values('2012-02-22', 2);
insert into DVDCopies(DatePurchased, DVDNumber) values('2017-05-13', 3);
insert into DVDCopies(DatePurchased, DVDNumber) values('2020-03-09', 2);
insert into DVDCopies(DatePurchased, DVDNumber) values('2015-02-01', 4);
insert into DVDCopies(DatePurchased, DVDNumber) values('2019-02-01', 5);
insert into DVDCopies(DatePurchased, DVDNumber) values('2019-11-21', 2);
insert into DVDCopies(DatePurchased, DVDNumber) values('2012-12-30', 2);
insert into DVDCopies(DatePurchased, DVDNumber) values('2021-02-01', 1);

insert into LoanTypes(LoanTypes, LoanDuration) values('monthly', 30);
insert into LoanTypes(LoanTypes, LoanDuration) values('daily', 1);
insert into LoanTypes(LoanTypes, LoanDuration) values('weekly', 7);

insert into MembershipCategories(MembershipCategoryDescription, MembershipCategoryTotalLoans) values('Premium', 10);
insert into MembershipCategories(MembershipCategoryDescription, MembershipCategoryTotalLoans) values('Normal', 5);
insert into MembershipCategories(MembershipCategoryDescription, MembershipCategoryTotalLoans) values('Medium', 7);

insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Ghale', 'Samrat', 'Kathmandu', '1999-12-01', 1);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Limbu', 'Ishan', 'Kathmandu', '2002-03-01', 2);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Gautam', 'Bishant', 'Kathmandu', '2009-10-01', 3);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Adhikari', 'Sugam', 'Kathmandu', '2010-07-01', 1);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Bohora', 'Arun', 'Kathmandu',    '2001-02-03', 2);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Giri', 'Rijan', 'Kathmandu',     '2001-01-01', 3);
insert into Members(MemberLastName, MemberFirstName, MemberAddress, MemberDOB, MembershipCategoryNumber) values('Shrestha', 'Sudip', 'Kathmandu',     '2001-01-01', 3);

insert into CastMembers(DVDNumber, ActorNumber) values(1, 1);
insert into CastMembers(DVDNumber, ActorNumber) values(2, 2);
insert into CastMembers(DVDNumber, ActorNumber) values(3, 3);
insert into CastMembers(DVDNumber, ActorNumber) values(4, 4);
insert into CastMembers(DVDNumber, ActorNumber) values(5, 5);
insert into CastMembers(DVDNumber, ActorNumber) values(1, 6);
insert into CastMembers(DVDNumber, ActorNumber) values(2, 7);
insert into CastMembers(DVDNumber, ActorNumber) values(3, 1);
insert into CastMembers(DVDNumber, ActorNumber) values(4, 2);
insert into CastMembers(DVDNumber, ActorNumber) values(5, 3);

insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-12', '2022-05-12', null, 1, 1, 1);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-13', '2022-05-13', null, 1, 2, 3);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-14', '2022-05-14', null, 2, 3, 2);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-15', '2022-05-15', null, 2, 4, 1);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-16', '2022-05-16', null, 1, 5, 2);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-17', '2022-05-17', null, 3, 6, 3);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-18', '2022-05-18', '2022-04-28', 1, 7, 3);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-19', '2022-05-19', '2022-05-11', 3, 8, 4);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-10', '2022-05-10', '2022-04-10', 2, 9, 4);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-04-11', '2022-05-11', '2022-04-11', 1, 10, 5);
insert into Loans(DateOut, DateDue, DateReturned, LoanTypeNumber, CopyNumber, MemberNumber) values('2022-01-18', '2022-02-18', '2022-02-28', 1, 7, 7);
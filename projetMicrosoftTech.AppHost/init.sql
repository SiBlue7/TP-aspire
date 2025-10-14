if not exists (select * from sysobjects where name='cars' and xtype='U') 
   create table car                                                      
   (                                                        
       id int                                                         
           constraint id_pk                                                           
               primary key                                           
   );
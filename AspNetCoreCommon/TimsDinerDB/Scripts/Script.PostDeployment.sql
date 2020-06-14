if not exists (select * from dbo.Food)
begin
    insert into dbo.Food(Title, [Description], Price)
    values ('Cheeseburger Meal', 'A cheeseburger, fries, and a drink', 5.95),
           ('Chili Dog Meal', 'Two chili dogs, fries, and a drink', 4.15),
           ('Vegan Meal', 'A large salad, and a water', 1.95);
end

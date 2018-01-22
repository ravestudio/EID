with source as
(select 

ROW_NUMBER() OVER(PARTITION BY code, operation order by dt) rn,

	  [OrderNumber]
      ,[Code]
      , dt
      ,[Operation]
      , price
      , count
      , summ
 from
(SELECT
	  [OrderNumber]
      ,[Code]
	  ,max(PARSE([Date] + ' ' + [Time] AS DATETIME USING 'en-gb')) dt
      ,[Operation]
      ,avg([Price]) price
      ,sum([Count]) count
      ,sum([Volume]) summ
  FROM [DealSet]
  group by OrderNumber, Code, Operation) temp)


  select * from source p1
  inner join source p2 on p1.rn = p2.rn and p1.Code = p2.Code and p1.Operation = 'Купля' and p2.Operation = 'Продажа'



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpendTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateSpend] [datetime] NOT NULL,
	[DaySpend] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
	[TransactionDescription] [nvarchar](255) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[WeekNumber] [int] NOT NULL,
	[UpTransactionId] [nvarchar](255) NULL,
	[CategorizedByAi] [bit] NOT NULL,
	[TransactionType] [nvarchar](255) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SpendTransactions] ADD PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

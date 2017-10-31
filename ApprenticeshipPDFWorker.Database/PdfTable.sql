CREATE TABLE [dbo].[PdfTable](
	[PrimaryKey] [int] IDENTITY(1,1) NOT NULL,
	[StandardCode] [int] NOT NULL,
	[StandardUrl] [varchar](max) NOT NULL,
	[AssessmentUrl] [varchar](max) NOT NULL,
	[DateSeen] [datetime] NOT NULL,
 CONSTRAINT [PK_PdfTable] PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[PdfTable] ADD  CONSTRAINT [DF_PdfTable]  DEFAULT (getdate()) FOR [DateSeen]
GO
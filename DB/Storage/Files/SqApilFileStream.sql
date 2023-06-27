
ALTER DATABASE [$(DatabaseName)]
ADD  FILE
(
	NAME = [SqApilFileStream],
	FILENAME = '$(DefaultLogPath)$(DefaultFilePrefix)_SqApilFileStream.ndf'
)

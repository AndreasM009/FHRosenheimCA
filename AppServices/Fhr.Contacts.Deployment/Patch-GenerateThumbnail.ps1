Param(
	[Parameter(Mandatory=$true)]
	$ConfigPath,
	[Parameter(Mandatory=$true)]
	$BuildConfiguration
)

# Production
if ($BuildConfiguration -ieq "release")
{
	[xml]$xml = Get-Content $ConfigPath
	($xml.configuration.connectionStrings.add | Where name -EQ 'ContactContext').connectionString = 'Data Source=tcp:fhrdbserver.database.windows.net;database=fhrdb;User ID=M009;Password=whiteduck123!;'
	($xml.configuration.appSettings.add | Where key -EQ 'thumbnailqueue').value = 'thumbnailqueue'
	($xml.configuration.appSettings.add | Where key -EQ 'thumbnailblob').value = 'thumbnailblob'
	$xml.Save($ConfigPath)
}

#Test
if ($BuildConfiguration -ieq "test")
{
	[xml]$xml = Get-Content $ConfigPath
	($xml.configuration.connectionStrings.add | Where name -EQ 'ContactContext').connectionString = 'Data Source=tcp:fhrdbserver.database.windows.net;database=fhrdb-test;User ID=M009;Password=whiteduck123!;'
	($xml.configuration.appSettings.add | Where key -EQ 'thumbnailqueue').value = 'thumbnailqueuetest'
	($xml.configuration.appSettings.add | Where key -EQ 'thumbnailblob').value = 'thumbnailblobtest'
	$xml.Save($ConfigPath)
}

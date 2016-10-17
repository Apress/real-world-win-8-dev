<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ApressCloudService" generation="1" functional="0" release="0" Id="5389557b-3916-4847-958f-b282e69b6f7d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ApressCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ApressODataService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ApressCloudService/ApressCloudServiceGroup/LB:ApressODataService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ApressODataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ApressCloudService/ApressCloudServiceGroup/MapApressODataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ApressODataServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ApressCloudService/ApressCloudServiceGroup/MapApressODataServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ApressODataService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapApressODataService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapApressODataServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ApressODataService" generation="1" functional="0" release="0" software="C:\Users\aapelpro\documents\visual studio 2012\Projects\ApressCloudService\ApressCloudService\csx\Release\roles\ApressODataService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ApressODataService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ApressODataService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="ApressODataService.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ApressODataServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ApressODataServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ApressODataServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="42d7da16-b6ce-4536-a4f1-a813f33d55ce" ref="Microsoft.RedDog.Contract\ServiceContract\ApressCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="e54174d0-972c-46ac-9171-bf4abb063ddc" ref="Microsoft.RedDog.Contract\Interface\ApressODataService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ApressCloudService/ApressCloudServiceGroup/ApressODataService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
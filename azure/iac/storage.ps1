$rg = 'aula-01-fia'

New-AzResourceGroupDeployment -ResourceGroupName $rg `
    -TemplateFile '.\storage.json' `
    -TemplateParameterFile '.\storage-values.json' 
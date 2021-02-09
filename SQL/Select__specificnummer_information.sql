use Numberservice;
Select 
nummer_information_ziel 
from nummer_information 
Where nummer_definition_id = 7 
And JSON_VALUE(Nnmmer_information_quelle, '$.Wert1') = 'abc' 
And JSON_VALUE(Nnmmer_information_quelle, '$.Wert2') = 13 
And JSON_VALUE(Nnmmer_information_quelle, '$.Wert3') = '0CE65519-99F7-4ADF-87C5-307B9FA9A813'
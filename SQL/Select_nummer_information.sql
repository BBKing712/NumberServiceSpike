use Numberservice;
select
nummer_information_id,
nummer_definition_id,
JSON_VALUE(Nnmmer_information_quelle, '$.Wert1')as Wert1,
JSON_VALUE(Nnmmer_information_quelle, '$.Wert2')as wert2,
JSON_VALUE(Nnmmer_information_quelle, '$.Wert3') as wert3
from nummer_information
where
nummer_definition_id = 7
and
JSON_VALUE(Nnmmer_information_quelle, '$.Wert1') = 'abc';
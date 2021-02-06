use Numberservice;
select
*
from nummer_definition a
Inner join nummer_definition_quelle b
on a.nummer_definition_id = b.nummer_definition_id;
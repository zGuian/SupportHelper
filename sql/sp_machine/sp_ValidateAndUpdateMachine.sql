CREATE OR REPLACE FUNCTION sp_ValidateAndUpdateMachine(
    m_id TEXT,
    m_hostname TEXT,
    m_current_username TEXT,
    m_domainName TEXT,
    m_operationalSystem TEXT,
    m_newId TEXT
)
RETURNS VOID AS $$
DECLARE
	m_id_exists BOOLEAN;
BEGIN
	SELECT EXISTS (
		SELECT 1 FROM machine WHERE id = m_id LIMIT 1
	)
	INTO m_id_exists;
	IF m_id_exists THEN
		UPDATE machine
		SET 
			hostname = m_hostname,
			current_username = m_current_username, 
			domain_name = m_domainName,
			operation_system = m_operationalSystem
		WHERE
            id = m_id;
	ELSE 
		INSERT INTO machine (id, hostname, current_username, domain_name, operational_system)
        VALUES (m_newId, m_hostname, m_current_username, m_domainName, m_operationalSystem);
    END IF;
END;
$$ LANGUAGE plpgsql;
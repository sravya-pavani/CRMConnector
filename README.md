# CRMConnector

## Overview

* This is a console application that connects to Dynamics CRM using Client ID and Client Secret generated through Azure AD.
* The application creates a record in contact entity. Also, it can associate with Account entity record which is recently modified.
* Retrieves all records based on a search string in an entity.
* Uploads data from CSV file to CRM in batches of 500 records at a time.

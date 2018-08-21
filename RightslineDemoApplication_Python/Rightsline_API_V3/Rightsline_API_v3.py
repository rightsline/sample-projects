import json, requests, hashlib, hmac, sys, datetime

with open('config.json', 'r') as file:
    data = json.loads(''.join(file.readlines()))

url = "https://api-dev.rightsline.com/v3/auth/temporary-credentials"
json_to_send = "{\n\t\"accessKey\":\"%s\",\n\t\"secretKey\":\"%s\"\n}" % (data["accessKey"], data["secretKey"])
initial_headers = {
    'content-type': "application/json",
    'x-api-key': data["xApiKey"]
}
response = requests.post(url, data=json_to_send, headers=initial_headers)
print(response.content.decode())
response = json.loads(response.content.decode())

accessKey = response["accessKey"]
secretKey = response["secretKey"]
securityToken = response["sessionToken"]
expiration = response["expiration"]
base_connection = "http://api-dev.rightsline.com/v3"
# method = 'GET'
service = 'execute-api'
region = "us-east-1"
host = 'api-dev.rightsline.com'
endpoint = "http://api-dev.rightsline.com/v3/catalog-item/1051"
endpoint1 = "http://api-dev.rightsline.com/v3/table/table-templates"


def generate_AWS_headers(method, request_params, payload=""):
    # Key derivation functions. See:
    # http://docs.aws.amazon.com/general/latest/gr/signature-v4-examples.html#signature-v4-examples-python
    def sign(key, msg):
        return hmac.new(key, msg.encode('utf-8'), hashlib.sha256).digest()

    def getSignatureKey(key, dateStamp, regionName, serviceName):
        kDate = sign(('AWS4' + key).encode('utf-8'), dateStamp)
        kRegion = sign(kDate, regionName)
        kService = sign(kRegion, serviceName)
        kSigning = sign(kService, 'aws4_request')
        return kSigning

    # Read AWS access key from env. variables or configuration file. Best practice is NOT
    # to embed credentials in code.
    access_key = accessKey
    secret_key = secretKey
    if access_key is None or secret_key is None:
        print('No access key is available.')
        sys.exit()

    # Create a date for headers and the credential string
    t = datetime.datetime.utcnow()
    amzdate = t.strftime('%Y%m%dT%H%M%SZ')
    datestamp = t.strftime('%Y%m%d')  # Date w/o time, used in credential scope

    # ************* TASK 1: CREATE A CANONICAL REQUEST *************
    # http://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html

    # Step 1 is to define the verb (GET, POST, etc.)--already done.

    # Step 2: Create canonical URI--the part of the URI from domain to query
    # string (use '/' if no path)
    canonical_uri = '/'

    # Step 3: Create the canonical query string. In this example (a GET request),
    # request parameters are in the query string. Query string values must
    # be URL-encoded (space=%20). The parameters must be sorted by name.
    # For this example, the query string is pre-formatted in the request_parameters variable.
    canonical_querystring = request_params

    # Step 4: Create the canonical headers and signed headers. Header names
    # must be trimmed and lowercase, and sorted in code point order from
    # low to high. Note that there is a trailing \n.
    if method != 'GET':
        canonical_headers = '\ncontent-type:application/json' + '\nhost:' + host + '\n' + 'x-amz-date:' + amzdate + '\n' + 'x-amz-security-token:' + securityToken + '\n' + 'x-api-key:' + \
                        data["xApiKey"] + "\n"
    else:
        canonical_headers = '\ncontent-type:' + '\nhost:' + host + '\n' + 'x-amz-date:' + amzdate + '\n' + 'x-amz-security-token:' + securityToken + '\n' + 'x-api-key:' + \
                            data["xApiKey"] + "\n"
    # Step 5: Create the list of signed headers. This lists the headers
    # in the canonical_headers list, delimited with ";" and in alpha order.
    # Note: The request can include any headers; canonical_headers and
    # signed_headers lists those that you want to be included in the
    # hash of the request. "Host" and "x-amz-date" are always required.
    signed_headers = 'content-type;host;x-amz-date;x-amz-security-token;x-api-key'

    # Step 6: Create payload hash (hash of the request body content). For GET
    # requests, the payload is an empty string ("").
    print(payload)
    if method == 'GET' or method == 'DELETE':
        payload_hash = hashlib.sha256().hexdigest()

    elif method == 'POST' or method == 'PUT':
        payload_hash = hashlib.sha256(payload.encode('utf-8')).hexdigest()

    # Step 7: Combine elements to create canonical request
    # print("QUERYSTRING: " + canonical_querystring)
    canonical_request = method + '\n' + canonical_uri + canonical_querystring + '\n' + canonical_headers + '\n' + signed_headers + '\n' + payload_hash
    # print("Canonical Request: " + canonical_request)
    # ************* TASK 2: CREATE THE STRING TO SIGN*************
    # Match the algorithm to the hashing algorithm you use, either SHA-1 or
    # SHA-256 (recommended)
    algorithm = 'AWS4-HMAC-SHA256'
    credential_scope = datestamp + '/' + region + '/' + service + '/' + 'aws4_request'
    string_to_sign = algorithm + '\n' + amzdate + '\n' + credential_scope + '\n' + hashlib.sha256(
        canonical_request.encode('utf-8')).hexdigest()
    # print("Sts: " + string_to_sign)
    # ************* TASK 3: CALCULATE THE SIGNATURE *************
    # Create the signing key using the function defined above.
    signing_key = getSignatureKey(secret_key, datestamp, region, service)

    # Sign the string_to_sign using the signing_key
    signature = hmac.new(signing_key, (string_to_sign).encode('utf-8'), hashlib.sha256).hexdigest()

    # ************* TASK 4: ADD SIGNING INFORMATION TO THE REQUEST *************
    # The signing information can be either in a query string value or in
    # a header named Authorization. This code shows how to use a header.
    # Create authorization header and add to request headers
    authorization_header = algorithm + ' ' + 'Credential=' + access_key + '/' + credential_scope + ', ' + 'SignedHeaders=' + signed_headers + ', ' + 'Signature=' + signature
    # print(authorization_header)
    # The request can include any headers, but MUST include "host", "x-amz-date",
    # and (for this scenario) "Authorization". "host" and "x-amz-date" must
    # be included in the canonical_headers and signed_headers, as noted
    # earlier. Order here is not significant.
    # Python note: The 'host' header is added automatically by the Python 'requests' library.
    headers = {"x-amz-date": amzdate, "x-api-key": data["xApiKey"], "content-type": "application/json",
               "x-amz-security-token": securityToken, "Authorization": authorization_header}

    return headers


def getCatalogItem():
    # request = requests.get(base_connection + "/catalog-item/" + str(catalog_item_number),
    #                        auth=HTTPBasicAuth(username, password))
    # print(request.content.decode())
    headers = generate_AWS_headers("GET", "v3/catalog-item/1051")
    returned = requests.get(endpoint, headers=headers)
    print(returned.content.decode())


def postCatalogItem():
    with open("Catalog Item Example JSON/CatalogItemFeaturePOST.json", 'r') as file:
        catalog_json = ''.join(file.readlines())[3:]
        print(catalog_json)
    headers = generate_AWS_headers("POST", "v3/catalog-item", payload=catalog_json)

    print(headers)
    returned = requests.post(base_connection + "/catalog-item", data=catalog_json, headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def putCatalogItem(catalog_item):
    with open("Catalog Item Example JSON/CatalogItemEpisodePUT.json", 'r') as file:
        catalog_json = ''.join(file.readlines())[3:]
        print(catalog_json)
    headers = generate_AWS_headers("PUT", "v3/catalog-item/" + str(catalog_item), payload=catalog_json)

    print(headers)
    returned = requests.put(base_connection + "/catalog-item/" + str(catalog_item), data=catalog_json, headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def deleteCatalogItem(catalog_item):
    headers = generate_AWS_headers("DELETE", "v3/catalog-item/" + str(catalog_item))
    print(headers)
    returned = requests.delete(base_connection + "/catalog-item/" + str(catalog_item), headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def getTable(table_id):
    headers = generate_AWS_headers("GET", "v3/table/" + str(table_id))
    returned = requests.get(base_connection + "/table/" + str(table_id), headers=headers)
    print(returned.content.decode())


def postTable():
    with open("Table Example JSON/TablePostExample.json", 'r') as file:
        table_json = ''.join(file.readlines())
        print(table_json)
    headers = generate_AWS_headers("POST", "v3/table", payload=table_json)

    print(headers)
    returned = requests.post(base_connection + "/table", data=table_json, headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def putTable(table_id):
    with open("Table Example JSON/TablePutExample.json", 'r') as file:
        table_json = ''.join(file.readlines())
        print(table_json)
    headers = generate_AWS_headers("PUT", "v3/table/" + str(table_id), payload=table_json)

    print(headers)
    returned = requests.put(base_connection + "/table/" + str(table_id), data=table_json, headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def deleteTable(table_id):
    headers = generate_AWS_headers("DELETE", "v3/table/" + str(table_id))
    print(headers)
    returned = requests.delete(base_connection + "/table/" + str(table_id), headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def getRelationship(relationship_id):
    headers = generate_AWS_headers("GET", "v3/relationship/" + str(relationship_id))
    returned = requests.get(base_connection + "/relationship/" + str(relationship_id), headers=headers)
    print(returned.content.decode())


def postRelationship():
    with open("Relationship Example JSON/RelationshipPost.json", 'r') as file:
        relationship_json = ''.join(file.readlines())
        print(relationship_json + "\n\n")
    headers = generate_AWS_headers("POST", "v3/relationship", payload=relationship_json)

    print(headers)
    returned = requests.post(base_connection + "/relationship", data=relationship_json, headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


def putRelationship():
    # Putting a Relationship is NOT a supported operation
    raise NotImplementedError


def deleteRelationship(relationship_id):
    headers = generate_AWS_headers("DELETE", "v3/relationship/" + str(relationship_id))
    print(headers)
    returned = requests.delete(base_connection + "/relationship/" + str(relationship_id), headers=headers)
    print(returned.content.decode().replace('\\n', '\n'))


# putCatalogItem(1541)
# postRelationship()
# getRelationship(48324735)
# postTable()
deleteTable(2678)
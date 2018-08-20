import requests, json
from requests.auth import HTTPBasicAuth

with open('config.json', 'r') as file:
    data = json.loads(''.join(file.readlines()))

base_connection = "http://api-qa.rightsline.com/v2"
username = data["user"]
password = data["pass"]


def getCatalogItem(catalog_item_number):
    request = requests.get(base_connection + "/catalog-item/" + str(catalog_item_number),
                           auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def postCatalogItem():
    # For this instance, we will use a predefined JSON file
    with open("Catalog Item Example JSON/CatalogItemEpisodePOST.json", 'r') as file:
        catalog_json = ''.join(file.readlines())
    request = requests.post(base_connection + "/catalog-item/", catalog_json, auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def putCatalogItem(catalog_item_number):
    with open("Catalog Item Example JSON/CatalogItemEpisodePUT.json", 'r') as file:
        catalog_json = ''.join(file.readlines())
    request = requests.put(base_connection + "/catalog-item/" + str(catalog_item_number), catalog_json,
                           auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def deleteCatalogItem(catalog_item_number):
    request = requests.delete(base_connection + "/catalog-item/" + str(catalog_item_number),
                              auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def getTable(table_number):
    request = requests.get(base_connection + "/table/" + str(table_number), auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def postTable():
    # For this instance, we will use a predefined JSON file
    with open("Table Example JSON/TablePostExample.json", 'r') as file:
        table_json = ''.join(file.readlines())
    request = requests.post(base_connection + "/table/", table_json, auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def putTable(table_number):
    with open("Table Example JSON/TablePutExample.json", 'r') as file:
        table_json = ''.join(file.readlines())
    request = requests.put(base_connection + "/table/" + str(table_number), table_json, auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def deleteTable(table_number):
    request = requests.delete(base_connection + "/table/" + str(table_number),
                              auth=HTTPBasicAuth(username, password))
    print(request.content.decode())
    

def getRelationship(relationship_number):
    request = requests.get(base_connection + "/relationship/" + str(relationship_number), auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def postRelationship():
    # For this instance, we will use a predefined JSON file
    with open("Relationship Example JSON/RelationshipPost.json", 'r') as file:
        relationship_json = ''.join(file.readlines())
    request = requests.post(base_connection + "/relationship/", relationship_json, auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


def putRelationship():
    # Putting a Relationship is NOT a supported operation
    raise NotImplementedError


def deleteRelationship(relationship_number):
    request = requests.delete(base_connection + "/relationship/" + str(relationship_number),
                              auth=HTTPBasicAuth(username, password))
    print(request.content.decode())


postRelationship()
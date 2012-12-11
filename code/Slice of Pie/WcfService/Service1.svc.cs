﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
  [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
  public class Service1 : IService1
  {
    static List<Byte> byteList = new List<Byte>();

    public void setValue(Byte bval)
    {
      byteList.Add(bval);
      Console.WriteLine("It works");
    }

    public Byte getValue(int index)
    {
      return byteList[index];
    }

    public int lengthOfVal()
    {
      return byteList.Count;
    }

    public String Welcome(String name)
    {
        return String.Format("Welcome {0}", name);
    }

    public Boolean AddUser(String email, String password)
    {
        return Server.Controller.GetInstance().AddUser(email, password);
    }

    public void AddFolder(String name, int parentFolderId)
    {
        Server.Controller.GetInstance().AddFolder(name, parentFolderId);
    }

    /// <summary>
    /// Adds a document to the database.
    /// </summary>
    /// <param name="name">The name of the document</param>
    /// <param name="userId">The id of the user that creates the document</param>
    /// <param name="folderId">The id of the folder in which the document is located</param>
    /// <param name="content">The content of the document</param>
    public void AddDocument(String name, int userId, int folderId, String content)
    {
        Server.Controller.GetInstance().AddDocument(name, userId, folderId, content);
    }

    public void AddDocumentRevision(int editorId, int documentId, String content)
    {
        Server.Controller.GetInstance().AddDocumentRevision(editorId, documentId, content);
    }

    public int GetUserByEmailAndPass(String email, String pass)
    {
        return Server.Controller.GetInstance().GetUser(email, pass);
    }

    public ServiceUser GetUserById(int userId)
    {
        return (ServiceUser)Server.Controller.GetInstance().GetUser(userId);
    }

    public ServiceUser GetUserByEmail(String email)
    {
        return (ServiceUser)Server.Controller.GetInstance().GetUser(email);
    }

    public ServiceDocument GetDocumentById(int documentId)
    {
        return (ServiceDocument)Server.Controller.GetInstance().GetDocument(documentId);
    }

    public ServiceDocument GetDocumentByName(String name)
    {
        return (ServiceDocument)Server.Controller.GetInstance().GetDocument(name);
    }

    public ServiceFolder GetFolder(int folderId)
    {
        return (ServiceFolder)Server.Controller.GetInstance().GetFolder(folderId);
    }

    public void DeleteFolder(int folderId)
    {
        Server.Controller.GetInstance().DeleteFolder(folderId);
    }

    public void DeleteDocumentReference(int userId, int documentId)
    {
        Server.Controller.GetInstance().DeleteDocumentReference(userId, documentId);
    }

    public void DeleteDocument(int documentId)
    {
        Server.Controller.GetInstance().DeleteDocument(documentId);
    }

  }
}
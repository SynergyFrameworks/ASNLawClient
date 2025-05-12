# ASN Law Reference System

## Overview
ASN Law Reference is a comprehensive document management system designed for legal professionals. It provides a robust platform for storing, viewing, searching, and annotating legal documents across various jurisdictions.

## Key Features

### Document Management
- **Browse Documents**: View all available legal documents organized by jurisdiction and category
- **Document Viewing**: Built-in PDF viewer for seamless document reading
- **Version Control**: Track different versions of documents with clear version indicators
- **Document Metadata**: Store and display comprehensive metadata including jurisdiction, creation date, and more

### Search Functionality
- **Full-Text Search**: Search across all documents using keywords or phrases
- **Advanced Filtering**: Filter search results by jurisdiction, document type, date range, tags and categories
- **Search Result Navigation**: Jump directly to pages containing search matches
- **Sort Options**: Arrange results by relevance, date, or title

### Annotation System
- **Create Annotations**: Add notes and comments to specific pages of documents
- **Visibility Control**: Choose to make annotations private or shared with other users
- **Annotation Management**: View all annotations for a document or filtered by page
- **Real-Time Updates**: Annotations appear immediately after creation

### Administrative Features
- **Document Upload**: Add new documents to the system with comprehensive metadata
- **Document Management**: Edit, update, or archive existing documents
- **User Management**: Control access and permissions (premium feature)
- **System Settings**: Configure API connections and display preferences

## Technical Architecture
- Built with Blazor WebAssembly for responsive client-side performance
- Integrates with PDF.js for document rendering
- RESTful API architecture for data access and management
- Configurable API endpoints for integration with different backend systems
- Local storage for user preferences and session data

## Getting Started
1. **Browse Documents**: Navigate to the Documents page to see all available documents
2. **Search**: Use the search bar at the top of most pages to find specific content
3. **View a Document**: Click on any document to open it in the document viewer
4. **Create Annotations**: Use the annotation panel (toggle visible/hidden) to add notes to documents
5. **Administrative Access**: Log in as an administrator to access document management features

## Technical Requirements
- Modern web browser (Chrome, Firefox, Edge, Safari)
- Internet connection for API access
- Administrator access for document upload and management features

## Future Enhancements
- Advanced analytics on document usage
- AI-powered document summarization
- Integration with case management systems
- Mobile application for on-the-go access
- Enhanced collaboration features including shared workspaces

## Development
To run the project locally:
```shell
git clone https://github.com/yourusername/asn-law-reference.git
cd asn-law-reference
dotnet restore
dotnet run

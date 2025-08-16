# Upload Directory Structure

## Directory Organization

### /Images
- **Products/**: Product images and photos
- **Categories/**: Category banner images
- **Blog/**: Blog post images and media
- **Thumbnails/**: Auto-generated thumbnails

### /Models
- **T-Shirt/**: 3D models for t-shirt products
- **Polo/**: 3D models for polo products
- **Hoodie/**: 3D models for hoodie products
- **Jacket/**: 3D models for jacket products
- **Shirt/**: 3D models for shirt products

### /Designs
- **Logos/**: User uploaded logos and designs
- **Custom/**: Custom design files
- **Previews/**: Generated design previews

### /Avatars
- User profile avatar images

## File Types Supported

### Images
- `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`
- Maximum size: 5MB
- Maximum dimensions: 5000x5000

### 3D Models
- `.glb`, `.gltf`, `.fbx`, `.obj`, `.dae`
- Maximum size: 50MB
- Validation: Structure and format checking

### Design Files
- `.jpg`, `.jpeg`, `.png`, `.gif`, `.svg`, `.ai`, `.psd`
- Maximum size: 10MB
- Auto-preview generation for images

## Usage

Upload files using the FileUploadHandler.ashx endpoint:

```javascript
// Upload product image
POST /Handlers/FileUploadHandler.ashx?type=image

// Upload 3D model
POST /Handlers/FileUploadHandler.ashx?type=3d

// Upload design file
POST /Handlers/FileUploadHandler.ashx?type=design

// Upload avatar
POST /Handlers/FileUploadHandler.ashx?type=avatar
```

## Security

- File type validation
- Size limits enforced
- Safe file naming (timestamp + GUID)
- Thumbnail generation for images
- 3D model structure validation

## File Naming Convention

Files are automatically renamed using:
`yyyyMMdd_HHmmss_[8-char-guid].[extension]`

Example: `20241208_143022_a1b2c3d4.jpg`